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
    bool TrySpawnUnit(int id, int shipLane);
    int GetCurrentEnergy();
    bool GetShowTip();
    void ToggleShowTip();
    IMothership GetMasterMothership();
    IMothership GetClientMothership();
    List<Ship> GetMyShips(int owner);
    List<Ship> GetEnemyShips(int owner);
}

public interface IGameScreenReceiver
{
    void OnDataChanged(ISpawnData[] data);
}