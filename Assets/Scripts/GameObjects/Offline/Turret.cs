using UnityEngine;
using System.Collections.Generic;

public class Turret : ClockMonoBehaviour
{
    public float ShootingAngle;
    public float ShootingDistance;

    private Ship _parentShip;
    private List<Ship> _ships = new List<Ship>();
    private Ship _target = null;
    private Rigidbody2D _targetRB = null;
    private Vector2 _shootDirection;
    
    public void Initialize(Ship parent, float fireRate)
    {
        _parentShip = parent;
        TickLength = 1f/fireRate;
    }

    public override void OnTick()
    {
        if(!PhotonNetwork.isMasterClient) return;

        if (_target == null) _selectTarget();
        if (_target == null) return;
        
        _shootDirection = _target.transform.position - transform.position;
        if (_shootDirection.magnitude > ShootingDistance) 
        {
            _target = null;
            _selectTarget();
        }
        if (Vector3.Angle(_parentShip.GetDirection(), _shootDirection) > ShootingAngle) return;

        _shootDirection = _shootAheadVector(_shootDirection, _target);
        PhotonNetwork.Instantiate("Projectile", Vector3.zero, Quaternion.identity, 0, 
            new object[] {
                _parentShip.viewID, _parentShip.GetUnitInfo().GetIndex(), _shootDirection, _parentShip.ownerId
            }
        );
    }

    private void _selectTarget()
    {
        if (_parentShip.EnemyMothership == null) return;
        _ships = _parentShip.EnemyMothership.GetShips();
            for (int shipI = 0; shipI < _ships.Count; shipI++)
            {
                if (_ships[shipI] == null) continue;
                _shootDirection = _ships[shipI].transform.position - transform.position;
                
                if (_shootDirection.magnitude > ShootingDistance) continue;
                if (Vector3.Angle(_parentShip.GetDirection(), _shootDirection) > ShootingAngle) continue;
                _target = _ships[shipI];
                _targetRB = null;
                return; 
            }
    }

    protected Vector3 _shootAheadVector(Vector3 initialDirection, Ship target)
    {
        return initialDirection;
        // if (_targetRB == null) _targetRB = target.GetComponent<Rigidbody2D>();
        // Vector3 shiftVector = _targetRB.velocity * (_shootDirection.magnitude / (ProjectilePrefab.Lifespan*800));
        // return initialDirection + shiftVector;
    }
}