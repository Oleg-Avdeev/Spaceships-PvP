using System.Collections.Generic;

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
    IMothership GetMasterMothership();
    IMothership GetClientMothership();
    List<Ship> GetMyShips();
    List<Ship> GetEnemyShips();
}

public interface IGameScreenReceiver
{
    void OnDataChanged(ISpawnData[] data);
}