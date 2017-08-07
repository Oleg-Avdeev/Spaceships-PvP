using UnityEngine;

public class Ship : PhotonView
{
    public int HP = 100;
    public float MaxTorque;
    public float MaxVelocity;
    public Turret[] Turrets;

    [HideInInspector]
    public Vector2 Direction { get; protected set; }
    private Vector3 _direction = Vector3.up;
    protected Rigidbody2D _rb2D;
    protected IUnitInfo _unit;
    protected Transform _transform;
    public IMothership MyMothership, EnemyMothership;


    private Vector3 _serverDirection = Vector3.zero;
    private Vector3 _serverPosition = Vector3.zero;
    private SpriteRenderer _spriteRenderer;


    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (instantiationData != null)
        {
            int id = (int)instantiationData[0];
            if (id < 0 || id >= Resolver.Instance.Units.Collection.Length)
            {
                PhotonNetwork.Destroy(gameObject);
                return;
            }

            _unit = Resolver.Instance.Units.Collection[id];

            gameObject.name = _unit.GetTitle() + "-for-" + ownerId;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _transform = transform;

            if (isMine)
            {
                MyMothership = Resolver.Instance.RoomController.GetMyMothership();
                EnemyMothership = Resolver.Instance.RoomController.GetEnemyMothership();
                _spriteRenderer.sprite = _unit.GetBlueIcon();
            }
            else
            {
                EnemyMothership = Resolver.Instance.RoomController.GetMyMothership();
                MyMothership = Resolver.Instance.RoomController.GetEnemyMothership();
                _spriteRenderer.sprite = _unit.GetRedIcon();
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

    public void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        if (MyMothership == null) return;
        MyMothership.GetShips().Add(this);

        MaxVelocity = GameController.Instance.MaxVelocity;//
        MaxTorque = GameController.Instance.MaxTorque;//

        for (int i = 0; i < Turrets.Length; i++)
            Turrets[i].Initialize(this);
    }

    public void LateUpdate()
    {
        // _fixPosition();

        // float k = _rb2D.velocity.magnitude / MaxVelocity;
        // _rb2D.AddForce (Direction * MaxTorque);
        // _rb2D.AddForce (-_rb2D.velocity * k);

        // float angle = Vector2.Angle(Vector2.up, _rb2D.velocity);
        // if (_rb2D.velocity.x > 0) angle = 360 - angle;
        // transform.rotation = Quaternion.Euler(0,0,angle);

        _direction = Vector3.RotateTowards(_direction, Direction, Time.deltaTime, 0).normalized;
        _transform.position += (MaxTorque * Time.deltaTime) * _direction;
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 90f;
        _transform.eulerAngles = new Vector3(0, 0, angle);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(Direction);
            // stream.SendNext(_direction);
            stream.SendNext(_transform.position);
        }
        else
        {
            Direction = (Vector2)stream.ReceiveNext();
            // _direction = (Vector3)stream.ReceiveNext();
            _serverPosition = (Vector3)stream.ReceiveNext();// + _direction * (MaxTorque * PhotonNetwork.GetPing() / 1000f);
            _transform.position = Vector3.Lerp(_transform.position, _serverPosition, 0.1f);
        }
    }

    private Projectile _incomingProjectile;
    void OnTriggerEnter2D(Collider2D other)
    {
        // if (!PhotonNetwork.isMasterClient) return;

        if (other.gameObject.CompareTag("Projectile"))
        {
            _incomingProjectile = other.GetComponent<Projectile>();
            if (_incomingProjectile.Owner != ownerId)
            {
                if (PhotonNetwork.isMasterClient)
                {
                    DealDamage(_incomingProjectile.Damage);
                }
                Destroy(other.gameObject);
            }
        }
    }

    public Rigidbody2D GetRigidBody()
    {
        return _rb2D;
    }

    public virtual void DealDamage(int damage)
    {
        HP -= damage;
        if (HP < 0)
        {
            // PhotonView _photonView = PhotonView.Get(this);
            photonView.RPC("_onDieMessageReceived", PhotonTargets.All);
        }
    }

    public virtual void Die()
    {
        // Show animation
    }

    [PunRPC]
    protected void _onDieMessageReceived()
    {
        if (isMine) PhotonNetwork.Destroy(gameObject);
        Die();
    }

}