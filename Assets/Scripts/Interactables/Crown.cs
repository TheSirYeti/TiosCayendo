using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Crown : MonoBehaviourPun, IPunObservable
{
    private Collider _collider;
    private bool hasWinner = false;
    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !hasWinner && other.GetComponent<PlayerController>().photonView.IsMine)
        {
            if (CurrentGameValues.instance != null)
                CurrentGameValues.instance.amWinner = true;
            
            photonView.RPC("RPC_DisableCollision", RpcTarget.All);
        }
    }
    [PunRPC]
    void RPC_DisableCollision()
    {
        Debug.Log("HAY GANADOR!");
        hasWinner = true;
        _collider.enabled = false;
        
        PhotonNetwork.LoadLevel("WinScene");
    }
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //
    }
}
