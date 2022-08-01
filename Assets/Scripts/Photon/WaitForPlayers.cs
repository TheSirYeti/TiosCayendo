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
    [SerializeField] private GameObject lostConnection;
    [SerializeField] private int amountOfLevels;

    private void Start()
    {
        SoundManager.instance.PlaySound(SoundID.TIO_FALL);
        Debug.Log("PLAYER COUNT: " + PhotonNetwork.PlayerList.Length);
        photonView.RPC("RPC_SetPlayerCount", RpcTarget.All, PhotonNetwork.PlayerList.Length);

        if (PhotonNetwork.PlayerList.Length == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            int rand = UnityEngine.Random.Range(1, amountOfLevels + 1);
            photonView.RPC("RPC_CheckFinalStatus", RpcTarget.AllBuffered, rand);
        }

        if (PhotonNetwork.PlayerList.Length == 1)
        {
            StartCoroutine(DoTimer(timeRemaining));
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //throw new NotImplementedException();
    }

    [PunRPC]
    void RPC_CheckFinalStatus(int levelID)
    {
        if (PhotonNetwork.PlayerList.Length <= 1)
        {
            PhotonNetwork.LoadLevel("MainMenu");
            return;
        }
        
        PhotonNetwork.LoadLevel("Level" + levelID);
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
                int rand = UnityEngine.Random.Range(1, amountOfLevels + 1);
                photonView.RPC("RPC_CheckFinalStatus", RpcTarget.AllBuffered, rand);
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
    void RPC_LostConnection()
    {
        lostConnection.SetActive(true);
    }
    
    
    private void OnApplicationQuit()
    {
        if (PhotonNetwork.LocalPlayer == PhotonNetwork.MasterClient)
        {
            photonView.RPC("RPC_LostConnection", RpcTarget.All);
        }
        PhotonNetwork.SendAllOutgoingCommands();
        PhotonNetwork.Disconnect();
    }
}
