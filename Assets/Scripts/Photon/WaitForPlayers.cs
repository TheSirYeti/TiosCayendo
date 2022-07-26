using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class WaitForPlayers : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private TextMeshProUGUI playerCountText;

    private void Start()
    {
        Debug.Log("PLAYER COUNT: " + PhotonNetwork.PlayerList.Length);
        photonView.RPC("RPC_SetPlayerCount", RpcTarget.All, PhotonNetwork.PlayerList.Length);
    }

    [PunRPC]
    void RPC_SetPlayerCount(int count)
    {
        Debug.Log("BANG!");
        playerCountText.text = count + " / 12";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        photonView.RPC("RPC_SetPlayerCount", RpcTarget.All, PhotonNetwork.PlayerList.Length);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new NotImplementedException();
    }
}
