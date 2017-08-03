public class NetworkManager : BaseNetworkManager, INetworkManager
{
    private ILobbyController _lobby = null;
    private IRoomController _room = null;

    void Start()
    {
        InvokeRepeating("CheckRoomPlayers", 1f, 1f);
    }

    public GameState GetState()
    {
        return _state;
    }

    public void Connect()
    {
        if (PhotonNetwork.connected == false)
        {
            PhotonNetwork.autoJoinLobby = false;
            PhotonNetwork.ConnectUsingSettings("0.1");
            _state = GameState.ConnectingToServer;
        }
    }

    public void JoinRoom(string uuid)
    {
        if (PhotonNetwork.inRoom && PhotonNetwork.room.Name.Equals(uuid) == false)
        {
            PhotonNetwork.LeaveRoom();
        }
        PhotonNetwork.JoinRandomRoom();
        _state = GameState.ConnectingToRoom;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void SetOnGameStateCallback(ILobbyController lobby)
    {
        _lobby = lobby;
    }

    public void SetOnGameStateCallback(IRoomController room)
    {
        _room = room;
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        base.OnPhotonRandomJoinFailed(codeAndMsg);

        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = Resolver.PLAYERS_COUNT }, null);
        _state = GameState.ConnectingToRoom;
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
    }

    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        base.OnPhotonJoinRoomFailed(codeAndMsg);

        if (_lobby != null) _lobby.OnFailedToConnect();
        if (_room != null) _room.OnDisconnected();
    }

    public override void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        base.OnFailedToConnectToPhoton(cause);

        if (_lobby != null) _lobby.OnFailedToConnect();
        if (_room != null) _room.OnDisconnected();
    }

    public override void OnConnectionFail(DisconnectCause cause)
    {
        base.OnConnectionFail(cause);

        if (_lobby != null) _lobby.OnFailedToConnect();
        if (_room != null) _room.OnDisconnected();
    }

    void CheckRoomPlayers()
    {
        if (_state == GameState.ConnectedToRoom)
        {
            if (PhotonNetwork.playerList.Length >= Resolver.PLAYERS_COUNT)
            {
                if (PhotonNetwork.isMasterClient)
                {
                    PhotonNetwork.room.IsOpen = false;
                }
                _state = GameState.GameStartedInRoom;

                if (_lobby != null) _lobby.OnGameStarted();
            }
        }
    }
}