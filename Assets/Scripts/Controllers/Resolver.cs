using UnityEngine;

public class Resolver {
	private static Resolver _instance = null;
	public static Resolver Instance {
		get {
			if(_instance == null)
			{
				_instance = new Resolver();
			}
			return _instance;
		}
	}

    public const int PLAYERS_COUNT = 2;

	private INetworkManager _networkManager = null;
	public INetworkManager NetworkManager
	{
		get {
			if(_networkManager == null)
			{
				_networkManager = GameObject.FindObjectOfType<NetworkManager>();
				if(_networkManager == null)
				{
					GameObject gameObject = new GameObject();
					gameObject.name = "NetworkManager";
					GameObject.DontDestroyOnLoad(gameObject);
					_networkManager = gameObject.AddComponent<NetworkManager>();
				}
			}
			return _networkManager;
		}
	}

	private IRoomController _roomController = null;
	public IRoomController RoomController
	{
		get {
			if(_roomController == null)
			{
				_roomController = GameObject.FindObjectOfType<RoomController>();
				if(_roomController == null)
				{
					GameObject gameObject = new GameObject();
					gameObject.name = "RoomController";
					GameObject.DontDestroyOnLoad(gameObject);
					_roomController = gameObject.AddComponent<RoomController>();
				}
			}
			return _roomController;
		}
	}

	private UnitsCollections _unitsCollection = null;
	public UnitsCollections Units
	{
		get {
			if(_unitsCollection == null)
			{
				_unitsCollection = Resources.Load<UnitsCollections>("UnitsCollections");
			}
			return _unitsCollection;
		}
	}
}
