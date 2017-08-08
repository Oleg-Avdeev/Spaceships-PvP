using UnityEngine;

public class Ship : PhotonView
{
    private float _health = 0;
    private bool _isDead = false;
    public Turret[] Turrets;

    protected Vector2 _targetDirection;
    protected IUnitInfo _unit;
    public IMothership MyMothership, EnemyMothership;

    protected Transform _transform;
    private Vector3 _direction = Vector3.up;

    private Vector3 _serverDirection = Vector3.zero;
    private Vector3 _serverPosition = Vector3.zero;
    private SpriteRenderer _spriteRenderer;
    private float _lastLogicUpdateTime;

    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (instantiationData != null)
        {
            int id = (int)instantiationData[0];
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

            if (isMine)
            {
                MyMothership = Resolver.Instance.RoomController.GetMyMothership();
                EnemyMothership = Resolver.Instance.RoomController.GetEnemyMothership();
            }
            else
            {
                EnemyMothership = Resolver.Instance.RoomController.GetMyMothership();
                MyMothership = Resolver.Instance.RoomController.GetEnemyMothership();
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
        if (MyMothership == null) return;
        MyMothership.GetShips().Add(this);

        for (int i = 0; i < Turrets.Length; i++)
            Turrets[i].Initialize(this);
    }

    public void Update()
    {
        if (isMine)
            if (Time.time - _lastLogicUpdateTime >= 0.1f)
            {
                _lastLogicUpdateTime = Time.time;
                LogicUpdate();
            }
    }

    public virtual void LogicUpdate() { }

    void LateUpdate()
    {
        _direction = Vector3.RotateTowards(_direction, _targetDirection, Time.deltaTime, 0).normalized;
        _direction.z = 0;
        
        _transform.position += (_unit.GetShipSpeed() * Time.deltaTime) * _direction;
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 90f;
        _transform.eulerAngles = new Vector3(0, 0, angle);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(_targetDirection);
            stream.SendNext(_transform.position);
        }
        else
        {
            _targetDirection = (Vector2)stream.ReceiveNext();
            _serverPosition = (Vector3)stream.ReceiveNext();// + _direction * (MaxTorque * PhotonNetwork.GetPing() / 1000f);
            _transform.position = Vector3.Lerp(_transform.position, _serverPosition, 0.7f);
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
                    DealDamage(projectile.Damage);
                }
                if(projectile.isMine)
                {
                    PhotonNetwork.Destroy(projectile);
                } else {
                    other.gameObject.SetActive(false);
                }
            }
        }
    }

    protected virtual void DealDamage(float damage)
    {
        _health -= damage;
        if (_health < 0 && !_isDead)
        {
            _isDead = true;
            photonView.RPC("OnDieMessageReceived", PhotonTargets.All);
        }
    }

    protected virtual void Die()
    {
        // Show animation
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

    public IUnitInfo GetUnitInfo()
    {
        return _unit;
    }

}