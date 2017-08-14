using UnityEngine;

public class Ship : PhotonView
{
	public TextMesh HealthText;
    protected float _health = 0;
    private bool _isDead = false;
    public Turret[] Turrets;
    public GameObject TextTipObject;

    protected Vector2 _targetDirection;
    protected IUnitInfo _unit;
    public IMothership MyMothership, EnemyMothership;
    public Explosion ExplosionPrefab;

    protected Transform _transform;
    private Vector3 _direction = Vector3.up;
    protected int _laneOffset;

    private Vector3 _serverDirection = Vector3.zero;
    private Vector3 _serverPosition = Vector3.zero;
    private SpriteRenderer _spriteRenderer;
    private float _lastLogicUpdateTime;

    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (instantiationData != null)
        {
            int id = (int)instantiationData[0];
            _laneOffset = (int)instantiationData[1];
            if (id < 0 || id >= Resolver.Instance.Units.Collection.Length)
            {
                if(isMine)
                {
                    PhotonNetwork.Destroy(gameObject);
                }
                return;
            }

            _unit = Resolver.Instance.Units.Collection[id];    
            if (ownerId != PhotonNetwork.masterClient.ID)
                _direction = Vector3.down;        
            ParseIUnitInfo();
            ColorShip();

            if (isMine == PhotonNetwork.isMasterClient)
            {
                MyMothership = Resolver.Instance.RoomController.GetMasterMothership();
                EnemyMothership = Resolver.Instance.RoomController.GetClientMothership();
            }
            else
            {
                EnemyMothership = Resolver.Instance.RoomController.GetMasterMothership();
                MyMothership = Resolver.Instance.RoomController.GetClientMothership();
            }
            
            _transform.position = MyMothership.GetTransform().position;
        }
        else
        {
            if(isMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
            return;
        }
    }

    protected void ParseIUnitInfo()
    {
        gameObject.name = _unit.GetTitle() + "-by-" + ownerId;
        _health = _unit.GetHealth();
        _isDead = false;
        _transform = transform;

        Debug.Log(_unit.GetTitle());
        if (_unit.GetTitle() == "Dreadnought") _direction = Vector3.zero;
    }

    protected void ColorShip()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (isMine)
            _spriteRenderer.sprite = _unit.GetBlueIcon();
        else
            _spriteRenderer.sprite = _unit.GetRedIcon();
            
    }

    void Start()
    {
        for (int i = 0; i < Turrets.Length; i++)
            Turrets[i].Initialize(this, _unit.GetFireRate());
        
        Color color;
        if (isMine) color = new Color(0, 0.9f,1, 1);
        else color = new Color(1,0.4f,0,1);

        TextTipObject.GetComponentInChildren<TextMesh>().color = color;
        var trail = GetComponentInChildren<TrailRenderer>();
        if (trail != null) trail.endColor = color;
        
        if (MyMothership == null) return;
            MyMothership.GetShips().Add(this);
    }

    public void Update()
    {
        if (isMine)
            if (Time.time - _lastLogicUpdateTime >= 0.1f)
            {
                _lastLogicUpdateTime = Time.time;
                LogicUpdate();
            }

		        
        if (HealthText != null)
            HealthText.text = _health + "/" + _unit.GetHealth();
        TextTipObject.SetActive(Resolver.Instance.RoomController.GetShowTip());
    }

    public virtual void LogicUpdate() { }

    void LateUpdate()
    {
        _direction = Vector2Extension.RotateTowards(_direction, _targetDirection, 2f).normalized;
        // _direction = Vector3.RotateTowards(_direction, _targetDirection, 0.1f, 0).normalized;
        
        _transform.position += (_unit.GetShipSpeed() * Time.deltaTime) * _direction * 2;
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 90f;
        _transform.eulerAngles = new Vector3(0, 0, angle);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(_targetDirection);
            stream.SendNext(_transform.position);
            stream.SendNext(_health);
        }
        else
        {
            Vector2 newTargetDirection = (Vector2)stream.ReceiveNext();
            float angle = 0.7f - Mathf.Clamp(Vector2.Angle(newTargetDirection, _targetDirection) / 45f, 0, 0.7f);
            _targetDirection = newTargetDirection;
            _serverPosition = (Vector3)stream.ReceiveNext();// + _direction * (MaxTorque * PhotonNetwork.GetPing() / 1000f);
            
            // _targetDirection = Vector3.Lerp(_targetDirection, newTargetDirection, angle);   
            _transform.position = Vector3.Lerp(_transform.position, _serverPosition, angle * 0.1f);
            
            // !!!
            if (PhotonNetwork.isMasterClient)   
                _health = (float)stream.ReceiveNext();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            Projectile projectile = other.GetComponent<Projectile>();
            if (projectile.Owner != ownerId)
            {
                if (PhotonNetwork.isMasterClient)
                {
                    // DealDamage(projectile.Damage);
                    photonView.RPC("DealDamage", PhotonTargets.All, new object[] { projectile.Damage });
                    projectile.Explode();
                }
                projectile.Delete();
            }
        }
    }

    [PunRPC]
    protected void DealDamage(float damage)
    {
        _health -= damage;
        if (_health < 0 && !_isDead)
        {
            _isDead = true;
            OnDieMessageReceived();
            // photonView.RPC("OnDieMessageReceived", PhotonTargets.All);
        }
    }

    protected virtual void Die()
    {
        // Show animation
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
    }

    [PunRPC]
    protected void OnDieMessageReceived()
    {
        if (isMine && gameObject != null) PhotonNetwork.Destroy(gameObject);
        Die();
    }

    public Vector3 GetDirection()
    {
        return _direction;
    }

    public Vector3 GetShipSpeed()
    {
        return (_unit.GetShipSpeed() * 2) * _direction;
    }

    public IUnitInfo GetUnitInfo()
    {
        return _unit;
    }

}