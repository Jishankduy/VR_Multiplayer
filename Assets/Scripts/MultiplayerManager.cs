using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

[System.Serializable]
public class DefaultRoom
{
    public string Name;
    public int sceneIndex;
    public int MaxPlayer;
}

public class MultiplayerManager : MonoBehaviourPunCallbacks
{
    public List<DefaultRoom> defaultRooms;
    public GameObject ConnectUI;
    public GameObject RoomUI;

    public void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();  // Connect to Photon servers
        Debug.Log("Try Connect To Server");
    }


    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Server");
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby(); // Join the default lobby

    }
    
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Joined the lobby.");
        RoomUI.SetActive(true);
        ConnectUI.SetActive(false);
    }
    
    public void InitiliazeRoom(int defaultRoomIndex)
    {
        DefaultRoom roomSetting = defaultRooms[defaultRoomIndex];

        //LOAD SCENE
        PhotonNetwork.LoadLevel(roomSetting.sceneIndex);

        //CREATE THE ROOM
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)roomSetting.MaxPlayer;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        PhotonNetwork.JoinOrCreateRoom("Room 1", roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a Room");
        base.OnJoinedRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("A new player joined the room");
        base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Failed to join room: {message}. Trying again...");
    }

}
