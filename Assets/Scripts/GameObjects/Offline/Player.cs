using UnityEngine;

public class Player : Ship {


    public Transform OrbitTarget;
    public float OrbitDistance = 10;

    private Vector3 _nextOrbitPoint;
   
    private void _createPoint()
    {
        if (_nextOrbitPoint == Vector3.zero)
        {
            _nextOrbitPoint = OrbitTarget.position + OrbitDistance*(transform.position - OrbitTarget.position).normalized;
            _nextOrbitPoint.z = transform.position.z;
        }
        else
        {
            _nextOrbitPoint = Quaternion.Euler(0,0,60) * _nextOrbitPoint;
        }
    }

    public override void LogicUpdate()
    {
        if (_nextOrbitPoint == Vector3.zero)
            _createPoint();
        if ((_nextOrbitPoint - transform.position).magnitude < 8.5)
            _createPoint();

        Debug.DrawLine(OrbitTarget.position, _nextOrbitPoint);

        _targetDirection = (_nextOrbitPoint - transform.position);
    }



}