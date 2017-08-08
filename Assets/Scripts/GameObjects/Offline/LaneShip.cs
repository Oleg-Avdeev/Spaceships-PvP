using System.Collections.Generic;
using UnityEngine;

public class LaneShip : Ship
{
    private Transform _target;
    private Ship _shipTarget;

    private bool _changingCourse;
    private float _timeForManeur = 0;
    private Vector3 _maneurVector;
    private Vector3 _followVector;
    private List<Ship> _ships = new List<Ship>();

    private Vector3 _screenPosition;
    private Vector3[] _orbitPoints;
    private int _pointsCount = 15;
    private int _laneOffset = 0;
    private int _currentPoint = 0;
    private int _attackDistance = 5;
   
    private void _createPoints()
    {
        //CORRECT
        Vector3 targetDirection = EnemyMothership.GetTransform().position - _transform.position;
        
        //TEST
        _laneOffset = Random.Range(0, 3) - 1;
        // _laneOffset = - 1;
        // Vector3 targetDirection = new Vector3(0,17,0) - _transform.position;
        
        float stepLength = targetDirection.magnitude / _pointsCount;

        _orbitPoints = new Vector3[_pointsCount];
        _orbitPoints[0] = _transform.position + _laneOffset*3*Vector3.right;
        for (int i = 1; i < _pointsCount; i++)
        {
            float dx = -_laneOffset * ((float)_pointsCount/2 - i)/2.2f;
            _orbitPoints[i] = _orbitPoints[i - 1] + targetDirection.normalized * stepLength - dx*Vector3.right;
        }
    }

    private void _updateTarget()
    {
        if (EnemyMothership == null)
        {
            return;
        }
        _ships = EnemyMothership.GetShips();
        for (int shipI = 0; shipI < _ships.Count; shipI++)
        {
            if (_ships[shipI] == null) continue;
            if ((_ships[shipI].transform.position - _transform.position).magnitude > _attackDistance) continue;
            _target = _ships[shipI].transform;
            _shipTarget = _ships[shipI];
            
            return;
        }
    }

    public override void LogicUpdate()
    {
        if (_targetDirection == Vector2.zero)
            _targetDirection = 5 * Vector2.up;

        if (!_isOnScreen())
        {
            _targetDirection = -_transform.position;
            return;
        }

        if (_target == null)
        {
            _updateTarget();
        }

        if (_target == null)
        {
            _followPath();
            return;
        } 

        if (_timeForManeur > 5)
        {
            _changingCourse = false;
            _timeForManeur = 0;
        }

        if (_changingCourse)
        {
            _executeManeur();
        }
        else
        {
            _maneurVector = Vector3.zero;
            _followTarget();
        }

        if (!_changingCourse && (_target.position - _transform.position).magnitude < 10)
        {
            _changingCourse = true;
            _maneurVector = Random.insideUnitSphere;
            _timeForManeur = -3;
        }
    }

    private void _followPath()
    {
            if (_currentPoint >= _pointsCount) return;

            if (_orbitPoints == null) _createPoints();
            
            // //DEBUG
            // for (int i = 1; i < _pointsCount; i++)
            //     Debug.DrawLine(_orbitPoints[i-1], _orbitPoints[i]);

            if ((_orbitPoints[_currentPoint] - _transform.position).magnitude < 7.5)
                if (_currentPoint < _pointsCount - 1)
                    _currentPoint++;

            _targetDirection = (_orbitPoints[_currentPoint] - _transform.position);
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
        _targetDirection = Vector2.MoveTowards(_targetDirection, _maneurVector, Mathf.PI / 10);
    }

    private void _followTarget()
    {
        // if (Direction == Vector2.zero) Direction = Vector2.left * MaxVelocity;
        // _followVector = Target.position - transform.position;
        _targetDirection = (_target.position - _shipTarget.GetDirection()) - transform.position;
        // Direction = Vector3.RotateTowards (Direction, _followVector, Mathf.PI / 10, 0);
    }
}