using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class FirstConnection : MonoBehaviourPunCallbacks
{
    public GameObject preLoadObjects, postLoadObjects;
    public GameObject loadingIcon, startButton;

    public void BTN_Connect()
    {
        startButton.SetActive(false);
        loadingIcon.SetActive(true);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        preLoadObjects.SetActive(false);
        postLoadObjects.SetActive(true);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Connection failed: {cause.ToString()}");
    }
}
