using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PreGameRoundup : MonoBehaviourPun, IPunObservable
{
    public float firstWaitTime = 3f;

    public GameObject loadingObjects, preGameObjects, inGameObjects, postGameObjects;

    private void Awake()
    {
        EventManager.Subscribe("OnRaceOver", OnRaceOver);
    }

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
        loadingObjects.SetActive(false);
        preGameObjects.SetActive(true);
    }

    [PunRPC]
    void RPC_StartGame()
    {
        EventManager.Trigger("OnPlayerMovementChanged", true);
        preGameObjects.SetActive(false);
        inGameObjects.SetActive(true);
    }

    void OnRaceOver(object[] parameters)
    {
        inGameObjects.SetActive(true);
        SoundManager.instance.StopAllMusic();
        SoundManager.instance.StopAllSounds();
        postGameObjects.SetActive(true);
    }
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //throw new NotImplementedException();
    }
}
