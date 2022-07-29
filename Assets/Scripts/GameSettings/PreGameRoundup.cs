using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PreGameRoundup : MonoBehaviourPun, IPunObservable
{
    public float firstWaitTime = 3f;

    public GameObject preGameObjects, inGameObjects, postGameObjects;

    private void Start()
    {
        if (PhotonNetwork.LocalPlayer == PhotonNetwork.MasterClient)
        {
            StartCoroutine(DoInitialBuffer());
        }
    }

    IEnumerator DoInitialBuffer()
    {
        yield return new WaitForSeconds(firstWaitTime);
        photonView.RPC("RPC_EnablePreGameObjects", RpcTarget.AllBuffered);
    }

    public void StartGame()
    {
        photonView.RPC("RPC_StartGame", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void RPC_EnablePreGameObjects()
    {
        preGameObjects.SetActive(true);
    }

    [PunRPC]
    void RPC_StartGame()
    {
        EventManager.Trigger("OnPlayerMovementChanged", true);
    }
    
    
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //throw new NotImplementedException();
    }
}
