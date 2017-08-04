using UnityEngine;
using System.Collections.Generic;
using System;

public class Turret : ClockMonoBehaviour
{

    public float ShootingAngle;
    public float ShootingDistance;
    public Projectile ProjectilePrefab;


    private Ship _parentShip;
    private List<Ship> _ships = new List<Ship>();
    private Ship _target = null;
    private Rigidbody2D _targetRB = null;
    private Vector2 _shootDirection;
    
    public void Initialize(Ship parent)
    {
        _parentShip = parent;
    }

    public override void OnTick()
    {
        if (_target == null) _selectTarget();
        if (_target == null) return;
        
        _shootDirection = _target.transform.position - transform.position;
        if (_shootDirection.magnitude > ShootingDistance) _selectTarget();
        if (Vector3.Angle(_parentShip.Direction, _shootDirection) > ShootingAngle) return;

        _shootDirection = _shootAheadVector(_shootDirection, _target);
        _shootDirection = _addErrorVector(_shootDirection, 1);
        ((Projectile)Instantiate(ProjectilePrefab, transform.position, new Quaternion()))
            .Fire(_shootDirection, _parentShip.ownerId);
    }

    private void _selectTarget()
    {
        _ships = ((MothershipController)_parentShip.EnemyMothership).Ships;
            for (int shipI = 0; shipI < _ships.Count; shipI++)
            {
                if (_ships[shipI] == null) continue;
                _shootDirection = _ships[shipI].transform.position - transform.position;
                if (_shootDirection.magnitude > ShootingDistance) continue;
                if (Vector3.Angle(_parentShip.Direction, _shootDirection) > ShootingAngle) continue;
                _target = _ships[shipI];
                _targetRB = null;
                return; 
            }
    }

    protected Vector3 _shootAheadVector(Vector3 initialDirection, Ship target)
    {
        if (_targetRB == null) _targetRB = target.GetComponent<Rigidbody2D>();
        Vector3 shiftVector = _targetRB.velocity * (_shootDirection.magnitude / (ProjectilePrefab.Lifespan*800));
        return initialDirection + shiftVector;
    }

    protected Vector3 _addErrorVector(Vector3 initialDirection, float errorAngle)
    {
        return Quaternion.Euler(0,0, UnityEngine.Random.Range(-errorAngle, errorAngle)) * initialDirection;
    }

}