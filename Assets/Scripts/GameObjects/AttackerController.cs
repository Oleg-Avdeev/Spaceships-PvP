using UnityEngine;

public class AttackerController : UnitController
{
    private Vector3 _speed = Vector3.zero;
    private Vector3 _serverSpeed = Vector3.zero;
    private Vector3 _serverPosition = Vector3.zero;

    void Update()
    {
        if (isMine)
        {
            Vector3 diff = Vector3.zero;
            if (_enemyMothership != null && _enemyMothership.GetTransform() != null)
            {
                diff = _enemyMothership.GetTransform().position - _transform.position;
            }
            else
            {
                diff = Vector3.zero - _transform.position;
            }
            _speed = _speed + Vector3.Normalize(diff) * 0.5f;
            if (_speed.sqrMagnitude > 2)
            {
                _speed = Vector3.Normalize(_speed) * 2;
            }
        }
        else
        {
            _speed = _serverSpeed;
        }
        _transform.position += _speed * Time.deltaTime;
        float angle = Mathf.LerpAngle(_transform.eulerAngles.z, Mathf.Atan2(_speed.y, _speed.x) * Mathf.Rad2Deg - 90, 0.5f);
        _transform.eulerAngles = new Vector3(0, 0, angle);

        if (PhotonNetwork.isMasterClient)
        {
            // Checks
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
}
