using UnityEngine;

public abstract class UnitController : PhotonView
{
    protected Transform _transform;
    protected IUnitInfo _unit;
    protected IMothership _myMothership, _enemyMothership;

    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (instantiationData != null)
        {
            int id = (int)instantiationData[0];
            if (id < 0 || id >= Resolver.Instance.Units.Collection.Length)
            {
                if(isMine)
                {
                    PhotonNetwork.Destroy(gameObject);
                }
                return;
            }

            _unit = Resolver.Instance.Units.Collection[id];

            gameObject.name = _unit.GetTitle() + "-for-" + ownerId;
            _transform = transform;

            if (isMine == PhotonNetwork.isMasterClient)
            {
                _myMothership = Resolver.Instance.RoomController.GetMasterMothership();
                _enemyMothership = Resolver.Instance.RoomController.GetClientMothership();
            }
            else
            {
                _enemyMothership = Resolver.Instance.RoomController.GetMasterMothership();
                _myMothership = Resolver.Instance.RoomController.GetClientMothership();
            }

            _transform.position = _myMothership.GetTransform().position;
        }
        else
        {
            if(isMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
            return;
        }
    }

}
