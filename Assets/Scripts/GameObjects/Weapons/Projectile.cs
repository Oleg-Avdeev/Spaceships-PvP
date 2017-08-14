using UnityEngine;

public class Projectile : PhotonView
{
    public float Damage { get; private set; }
    public float Speed { get; private set; }
    public float Lifespan;
    public Explosion ExplosionPrefab;

    public int Owner { get; private set; }
    protected Vector3 _direction;
    protected int _viewID; 
    protected int _shipIndex; 

    // public Color RedColor;

    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (instantiationData != null)
        {
            _viewID = (int)instantiationData[0];
            _shipIndex = (int)instantiationData[1];

            PhotonView view = PhotonView.Find((int)instantiationData[0]);
            if (view == null)
            {
                if (isMine)
                {
                    PhotonNetwork.Destroy(gameObject);
                }
                return;
            }
            transform.position = view.transform.position;
            
            IUnitInfo unit = Resolver.Instance.Units.Collection[_shipIndex];
            Damage = unit.GetDamage();
            Speed = unit.GetProjectileSpeed();
            Owner = (int)instantiationData[3];

            Fire((Vector2)instantiationData[2]);
        }
        else
        {
            if (isMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
            return;
        }
    }


    protected virtual void Fire(Vector2 direction)
    {
        _direction = Speed * direction.normalized;
        float angle = Vector3.Angle(Vector3.up, _direction) - 90;
        if (_direction.x > 0) angle = 360 - angle;
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    public void Update()
    {
        transform.position += _direction * Time.deltaTime;
        Lifespan -= Time.deltaTime;
        if (Lifespan <= 0)
        {
            if (isMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    public virtual void Explode()
    {
        photonView.RPC("SyncExplode", PhotonTargets.All);
    }

    public virtual void Delete()
    {
        if(isMine)
        {
            PhotonNetwork.Destroy(gameObject);
        } else {
            gameObject.SetActive(false);
        }
    }

    [PunRPC]
    protected virtual void SyncExplode()
    {
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
    }
}