using System.Collections.Generic;
using UnityEngine;

public class FighterShip : Ship {


    public Transform Target;

    private bool _changingCourse;
    private float _timeForManeur = 0;
    private Vector3 _maneurVector;
    private Vector3 _followVector;
    private List<Ship> _ships = new List<Ship>();
    private Rigidbody2D _shipTargetRB;

    private Vector3 _screenPosition;


    private void _updateTarget()
    {
        _ships = ((MothershipController)EnemyMothership).Ships;
        for (int shipI = 0; shipI < _ships.Count; shipI++)
        {
            if (_ships[shipI] == null) continue;
            // Check Distance
            Target = _ships[shipI].transform;
            _shipTargetRB = _ships[shipI].GetRigidBody();
            return; 
        }
    }

    public void Update()
    {
        if (Direction == Vector2.zero)
            Direction = 5*Vector3.up;
        
        if (!_isOnScreen())
        {
            Direction = Vector2.MoveTowards (Direction, -(Vector2)transform.position, Mathf.PI / 10);
            return;
        }

        if (Target == null) _updateTarget();
        if (Target == null) return;

		if (_timeForManeur > 5) {
			_changingCourse = false;
			_timeForManeur = 0;
		}

		if (_changingCourse) {
			_executeManeur ();
		} else {
			_maneurVector = Vector3.zero;
			_followTarget ();
		}

        if (!_changingCourse && (Target.position - transform.position).magnitude < 10)
        {
            _changingCourse = true;
            _maneurVector = Random.insideUnitSphere;
            _timeForManeur = -3;
        }
	}

    private bool _isOnScreen()
    {
        _screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return !(_screenPosition.x > Screen.width
                || _screenPosition.x < 0
                 || _screenPosition.y > Screen.height
                  || _screenPosition.y < 0);
    }

    private void _executeManeur()
    {
		_timeForManeur += 0.2f;
		Direction = Vector2.MoveTowards (Direction, _maneurVector, Mathf.PI / 10);
    }
    
    private void _followTarget()
    {
        // if (Direction == Vector2.zero) Direction = Vector2.left * MaxVelocity;
        // _followVector = Target.position - transform.position;
        Direction = (Target.position - (Vector3)_shipTargetRB.velocity ) - transform.position;
		// Direction = Vector3.RotateTowards (Direction, _followVector, Mathf.PI / 10, 0);
    }
}