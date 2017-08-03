public enum GameState
{
    NotConnected,
    ConnectingToServer,
    ConnectedToServer,
    ConnectingToRoom,
    ConnectedToRoom,
    GameStartedInRoom
}

public interface INetworkManager
{
	GameState GetState();
    void Connect();
    void JoinRoom(string uuid);
    void LeaveRoom();
    void SetOnGameStateCallback(ILobbyController lobby);
    void SetOnGameStateCallback(IRoomController room);
}

public interface ILobbyController
{
    void OnGameStarted();
    void OnFailedToConnect();
}

public interface IRoomController
{
    void StartGame();
    void OnDisconnected();
    void SetOnDataChangedReceiver(IGameScreenReceiver data);
    void TrySpawnUnit(int id);
    IMothership GetMyMothership();
    IMothership GetEnemyMothership();
}

public interface IGameScreenReceiver
{
    void OnDataChanged(ISpawnData[] data);
}