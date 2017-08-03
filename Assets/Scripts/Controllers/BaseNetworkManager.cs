using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class BaseNetworkManager : Photon.PunBehaviour {
    protected GameState _state = GameState.NotConnected;

	public override void OnConnectedToPhoton()
	{
		Debug.Log("OnConnectedToPhoton");
        _state = GameState.ConnectedToServer;
	}

    public override void OnLeftRoom()
	{
		Debug.Log("OnLeftRoom");
        _state = GameState.ConnectingToRoom;
	}

    public override void OnMasterClientSwitched(PhotonPlayer newMasterClient)
	{
		Debug.Log("OnMasterClientSwitched");
	}

    public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
	{
		Debug.Log("OnPhotonCreateRoomFailed");
        _state = GameState.ConnectedToServer;
	}

    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
	{
		Debug.Log("OnPhotonJoinRoomFailed");
        _state = GameState.ConnectedToServer;
	}

    public override void OnCreatedRoom()
	{
		Debug.Log("OnCreatedRoom");
	}

    public override void OnJoinedLobby()
	{
		Debug.Log("OnJoinedLobby");
        _state = GameState.ConnectedToServer;
	}

    public override void OnLeftLobby()
	{
		Debug.Log("OnLeftLobby");
        _state = GameState.ConnectedToServer;
	}

    public override void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		Debug.Log("OnFailedToConnectToPhoton");
        _state = GameState.NotConnected;
	}

    public override void OnConnectionFail(DisconnectCause cause)
	{
		Debug.Log("OnConnectionFail");
        _state = GameState.NotConnected;
	}

    public override void OnDisconnectedFromPhoton()
	{
		Debug.Log("OnDisconnectedFromPhoton");
        _state = GameState.NotConnected;
	}

    public override void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		Debug.Log("OnPhotonInstantiate");
	}

    public override void OnReceivedRoomListUpdate()
	{
		Debug.Log("OnReceivedRoomListUpdate");
	}

    public override void OnJoinedRoom()
	{
		Debug.Log("OnJoinedRoom");
        _state = GameState.ConnectedToRoom;
	}

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
	{
		Debug.Log("OnPhotonPlayerConnected");
	}

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
	{
		Debug.Log("OnPhotonPlayerDisconnected");
	}

	public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
	{
		Debug.Log("OnPhotonRandomJoinFailed");
	}

    public override void OnConnectedToMaster()
	{
		Debug.Log("OnConnectedToMaster");
        _state = GameState.ConnectedToServer;
	}

    public override void OnPhotonMaxCccuReached()
	{
		Debug.Log("OnPhotonMaxCccuReached");
	}

    public override void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged)
	{
		Debug.Log("OnPhotonCustomRoomPropertiesChanged");
	}

	public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
	{
		Debug.Log("OnPhotonPlayerPropertiesChanged");
	}

    public override void OnUpdatedFriendList()
	{
		Debug.Log("OnUpdatedFriendList");
	}

    public override void OnCustomAuthenticationFailed(string debugMessage)
	{
		Debug.Log("OnCustomAuthenticationFailed");
	}
    public override void OnCustomAuthenticationResponse(Dictionary<string, object> data)
	{
		Debug.Log("OnCustomAuthenticationResponse");
	}

    public override void OnWebRpcResponse(OperationResponse response)
	{
		Debug.Log("OnWebRpcResponse");
	}

    public override void OnOwnershipRequest(object[] viewAndPlayer)
	{
		Debug.Log("OnOwnershipRequest");
	}

    public override void OnLobbyStatisticsUpdate()
	{
		Debug.Log("OnLobbyStatisticsUpdate");
	}

	public override void OnPhotonPlayerActivityChanged(PhotonPlayer otherPlayer)
	{
		Debug.Log("OnPhotonPlayerActivityChanged");
	}

	public override void OnOwnershipTransfered(object[] viewAndPlayers)
	{
		Debug.Log("OnOwnershipTransfered");
	}
}
