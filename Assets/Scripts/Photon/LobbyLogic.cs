using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class LobbyLogic : MonoBehaviourPunCallbacks
{
    public void DoJoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    
    public override void OnCreatedRoom()
    {
        Debug.Log("Room Created");
    }
    
    
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("WaitingForPlayers");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 12;
        
        PhotonNetwork.CreateRoom(null, options, TypedLobby.Default);
    }
}
