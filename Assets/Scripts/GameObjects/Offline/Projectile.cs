using UnityEngine;

public class Projectile : PhotonView
{
    public float Damage { get; private set; }
    public float Speed { get; private set; }
    public float Lifespan;

    public int Owner { get; private set; }
    private Vector3 _direction;

    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (instantiationData != null)
        {
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

            Fire((int)instantiationData[1], (Vector2)instantiationData[2], (int)instantiationData[3]);
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


    public void Fire(int unitId, Vector3 direction, int owner)
    {
        IUnitInfo unit = Resolver.Instance.Units.Collection[unitId];
        Damage = unit.GetDamage();
        Speed = unit.GetProjectileSpeed();
        Owner = owner;

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

}