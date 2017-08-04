using UnityEngine;

public class Ship : PhotonView
{
    public int HP = 100;
    public float MaxTorque;
    public float MaxVelocity;
    public Turret[] Turrets;

    [HideInInspector] public Vector2 Direction;
    protected Rigidbody2D _rb2D;
    protected IUnitInfo _unit;
    protected Transform _transform;
    public IMothership MyMothership, EnemyMothership;


    private Vector3 _speed = Vector3.zero;
    private Vector3 _serverSpeed = Vector3.zero;
    private Vector3 _serverPosition = Vector3.zero;
    private Vector3 _diff;


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
            _transform = transform;

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
            PhotonNetwork.Destroy(gameObject);
            return;
        }
    }

    public void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        ((MothershipController)MyMothership).Ships.Add(this);

        for (int i = 0; i < Turrets.Length; i++)
            Turrets[i].Initialize(this);
    }

    public void FixedUpdate()
    {
        _fixPosition();

        float k = _rb2D.velocity.magnitude / MaxVelocity;
		_rb2D.AddForce (Direction * MaxTorque);
		_rb2D.AddForce (-_rb2D.velocity * k);

        float angle = Vector2.Angle(Vector2.up, _rb2D.velocity);
        if (_rb2D.velocity.x > 0) angle = 360 - angle;
		transform.rotation = Quaternion.Euler(0,0,angle);
    }

    private void _fixPosition()
    {
        if (isMine)
        {
            _diff = Vector3.zero;
            if (EnemyMothership != null && EnemyMothership.GetTransform() != null)
            {
                _diff = EnemyMothership.GetTransform().position - _transform.position;
            }
            else
            {
                _diff = Vector3.zero - _transform.position;
            }
            _speed = _speed + Vector3.Normalize(_diff) * 0.5f;
            if (_speed.sqrMagnitude > 2)
            {
                _speed = Vector3.Normalize(_speed) * 2;
            }
        }
        else
        {
            _speed = _serverSpeed;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(_speed);
            stream.SendNext(_transform.position);
        }
        else
        {
            _serverSpeed = (Vector3)stream.ReceiveNext();
            _serverPosition = (Vector3)stream.ReceiveNext();
            _transform.position = Vector3.Lerp(_transform.position, _serverPosition, 0.5f);
        }
    }

    private Projectile _incomingProjectile;
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Projectile")
        {
            _incomingProjectile = other.GetComponent<Projectile>();
            if (_incomingProjectile.Owner != ownerId)
            {
                DealDamage(_incomingProjectile.Damage);
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
        if (HP < 0) Die();
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}