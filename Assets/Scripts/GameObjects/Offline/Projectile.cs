using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int Damage = 10;
    public float Speed = 10;
    public float Lifespan;
    
    [HideInInspector] public int Owner;
    private Vector3 _direction;
    
    
    public void Fire(Vector3 direction, int owner)
    {
        _direction = Speed*direction.normalized;
        float angle = Vector3.Angle (Vector3.up, _direction) - 90;
        if (_direction.x > 0) angle = 360 - angle;
        transform.localRotation = Quaternion.Euler(0,0, angle);
    
        Owner = owner;
    }

    public void FixedUpdate()
    {
        transform.position += _direction;
        Lifespan -= Time.deltaTime;
        if (Lifespan <= 0)
            Destroy(gameObject);
    }

}