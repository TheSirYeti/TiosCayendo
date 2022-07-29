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
    [SerializeField] private float timeRemaining = 60f;
    [SerializeField] private TextMeshProUGUI timeText;
    
    private void Start()
    {
        SoundManager.instance.PlaySound(SoundID.TIO_FALL);
        Debug.Log("PLAYER COUNT: " + PhotonNetwork.PlayerList.Length);
        photonView.RPC("RPC_SetPlayerCount", RpcTarget.All, PhotonNetwork.PlayerList.Length);

        if (PhotonNetwork.PlayerList.Length == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            photonView.RPC("RPC_CheckFinalStatus", RpcTarget.AllBuffered);
        }

        if (PhotonNetwork.PlayerList.Length == 1)
        {
            StartCoroutine(DoTimer(timeRemaining));
            //photonView.RPC("RPC_UpdateTimer", RpcTarget.All, timeRemaining);
        }
    }

    [PunRPC]
    void RPC_SetPlayerCount(int count)
    {
        playerCountText.text = count + " / 12";
    }

    [PunRPC]
    void RPC_StartTimer()
    {
        StartCoroutine(DoTimer(timeRemaining));
    }
    
    public override void OnDisconnected(DisconnectCause cause)
    {

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //throw new NotImplementedException();
    }

    [PunRPC]
    void RPC_CheckFinalStatus()
    {
        if(PhotonNetwork.PlayerList.Length <= 1)
            PhotonNetwork.LoadLevel("MainMenu");

        else PhotonNetwork.LoadLevel("TestMovement");
    }

    [PunRPC]
    void RPC_UpdateTimer(float time)
    {
        timeRemaining = time;
        timeText.text = time.ToString();
    }

    IEnumerator DoTimer(float time)
    {
        bool flag = false;
        
        while (!flag)
        {
            yield return new WaitForSeconds(1f);
            time -= 1;
            timeText.text = time.ToString();
        
            if (time <= 0)
            {
                photonView.RPC("RPC_CheckFinalStatus", RpcTarget.AllBuffered);
                flag = true;
            }
            else
            {
                photonView.RPC("RPC_UpdateTimer", RpcTarget.AllBuffered, time);
            }

            yield return new WaitForEndOfFrame();
        }
    }

    [PunRPC]
    void RPC_TEST()
    {
        Debug.Log("PLAYER QUIT!");
        photonView.RPC("RPC_SetPlayerCount", RpcTarget.All, PhotonNetwork.PlayerList.Length - 1);
        if (PhotonNetwork.LocalPlayer == PhotonNetwork.MasterClient)
        {
            StopCoroutine(DoTimer(timeRemaining));
            StartCoroutine(DoTimer(timeRemaining)); 
        }
    }
    
    
    private void OnApplicationQuit()
    {
        photonView.RPC("RPC_TEST", RpcTarget.All);
        PhotonNetwork.SendAllOutgoingCommands();
        PhotonNetwork.Disconnect();
    }
}
