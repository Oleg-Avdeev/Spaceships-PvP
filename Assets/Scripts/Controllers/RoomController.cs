using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class RoomController : MonoBehaviour, IRoomController
{
    private IGameScreenReceiver _dataReceiver = null;
    private MothershipController _masterMothership = null, _clientMothership = null;
	private List<Ship> _masterShips = new List<Ship>();
	private List<Ship> _clientShips = new List<Ship>();

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
        {
            Camera.main.transform.localRotation = Quaternion.Euler(0,0,180);
            Camera.main.transform.localPosition = new Vector3(0,3,-15.2f);
        }
        else
        {
            Camera.main.transform.localRotation = Quaternion.identity;
            Camera.main.transform.localPosition = new Vector3(0,-3,-15.2f);
        } 

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
            if (_masterMothership.TrySpawnUnit(id))
            {
                if (_dataReceiver != null)
                {
                    _dataReceiver.OnDataChanged(_masterMothership.GetSpawnData());
                }
            }
        }
    }

    public IMothership GetMasterMothership()
    {
        return (IMothership)_masterMothership;
    }

    public IMothership GetClientMothership()
    {
        return (IMothership)_clientMothership;
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
                    if (PhotonNetwork.isMasterClient == motherships[i].isMine)
                        _masterMothership = motherships[i];
                    else 
                        _clientMothership = motherships[i];
                    
                }

                _isRoomReadyToPlay = true;
                if (_dataReceiver != null)
                {
                    _dataReceiver.OnDataChanged(_masterMothership.GetSpawnData());
                }
                break;
            }
        }
    }

    public List<Ship> GetMyShips()
    {
        if (PhotonNetwork.isMasterClient) return _masterShips;
        else return _clientShips;
    }

    public List<Ship> GetEnemyShips()
    {
        if (PhotonNetwork.isMasterClient) return _clientShips;
        else return _masterShips;
    }

    private void ClearRoom()
    {
        StopAllCoroutines();
        _clientShips.Clear();
        _masterShips.Clear();
        _isRoomReadyToPlay = false;
    }
}
