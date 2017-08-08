using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class RoomController : MonoBehaviour, IRoomController
{
    private IGameScreenReceiver _dataReceiver = null;
    private MothershipController _myMothership = null, _enemyMothership = null;
	private List<Ship> _myShips = new List<Ship>();
	private List<Ship> _enemyShips = new List<Ship>();

    private Coroutine _checkInstantiationsCoroutine = null;
    private bool _isRoomReadyToPlay = false;

    public void StartGame()
    {
        Resolver.Instance.NetworkManager.SetOnGameStateCallback(this);
        if (_dataReceiver != null)
        {
            _dataReceiver.OnDataChanged(null);
        }

		Debug.Log("Instantiate Mothership for " + PhotonNetwork.player.ID);
        
        PhotonNetwork.Instantiate("Dreadnought", 
        Vector3.zero,
        Quaternion.identity, 0, new object[] { 5, new int[] { 0, 1, 2, 3, 4 } });

        if (!PhotonNetwork.isMasterClient)
            Camera.main.transform.localRotation = Quaternion.Euler(0,0,180);
        else Camera.main.transform.localRotation = Quaternion.identity;

        _isRoomReadyToPlay = false;
        _checkInstantiationsCoroutine = StartCoroutine("CheckInstantiations");
    }

    public void OnDisconnected()
    {
        Debug.Log("OnDisconnected");
        ClearRoom();
    }

    public void SetOnDataChangedReceiver(IGameScreenReceiver data)
    {
        _dataReceiver = data;
    }

    public void TrySpawnUnit(int id)
    {
        if (_isRoomReadyToPlay)
        {
            if (_myMothership.TrySpawnUnit(id))
            {
                if (_dataReceiver != null)
                {
                    _dataReceiver.OnDataChanged(_myMothership.GetSpawnData());
                }
            }
        }
    }

    public IMothership GetMyMothership()
    {
        return (IMothership)_myMothership;
    }

    public IMothership GetEnemyMothership()
    {
        return (IMothership)_enemyMothership;
    }


    private IEnumerator CheckInstantiations()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            MothershipController[] motherships = FindObjectsOfType<MothershipController>();
            if (motherships.Length >= Resolver.PLAYERS_COUNT)
            {
                for (int i = 0; i < motherships.Length; i++)
                {
                    if (motherships[i].isMine)
                    {
                        _myMothership = motherships[i];
                    }
                    if (!motherships[i].isMine)
                    {
                        _enemyMothership = motherships[i];
                    }
                }

                _isRoomReadyToPlay = true;
                if (_dataReceiver != null)
                {
                    _dataReceiver.OnDataChanged(_myMothership.GetSpawnData());
                }
                break;
            }
        }
    }

    public List<Ship> GetMyShips()
    {
        return _myShips;
    }

    public List<Ship> GetEnemyShips()
    {
        return _enemyShips;
    }

    private void ClearRoom()
    {
        StopAllCoroutines();
        _isRoomReadyToPlay = false;
    }
}
