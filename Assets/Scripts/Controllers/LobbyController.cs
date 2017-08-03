using UnityEngine;

public class LobbyController : MonoBehaviour, ILobbyController
{
	void Start()
	{
		Resolver.Instance.NetworkManager.SetOnGameStateCallback(this);
	}

    void OnGUI()
    {
        switch (Resolver.Instance.NetworkManager.GetState())
        {
            case GameState.NotConnected:
                if (GUILayout.Button("ConnectToServer", GUILayout.Width(200), GUILayout.Height(100)))
                {
                    Resolver.Instance.NetworkManager.Connect();
                }
                break;
            case GameState.ConnectedToServer:
                if (GUILayout.Button("Join Room", GUILayout.Width(200), GUILayout.Height(100)))
                {
                    Resolver.Instance.NetworkManager.JoinRoom(System.Guid.NewGuid().ToString());
                }
                break;
            case GameState.ConnectedToRoom:
            case GameState.GameStartedInRoom:
                if (GUILayout.Button("LeaveRoom", GUILayout.Width(200), GUILayout.Height(100)))
                {
                    Resolver.Instance.NetworkManager.LeaveRoom();
                }
                GUILayout.Label(PhotonNetwork.room.Name + "/" + PhotonNetwork.isMasterClient);
                break;
        }
    }

    public void OnGameStarted()
	{
		Resolver.Instance.RoomController.StartGame();
	}

    public void OnFailedToConnect()
	{
		Debug.Log("Failed to connect!");
	}

}
