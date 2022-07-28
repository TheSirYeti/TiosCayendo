using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class GetWinner : MonoBehaviourPun, IPunObservable
{
    public TextMeshProUGUI nameDisplay;
    public TextMeshProUGUI messageDisplay;

    private void Start()
    {
        if (CurrentGameValues.instance == null)
        {
            Debug.Log("NO HAY GAME");
            return;
        }

        if (CurrentGameValues.instance.amWinner)
        {
            string name = PlayerPrefs.GetString("PREFS_PlayerName");
            photonView.RPC("RPC_DisplayWinnerName", RpcTarget.AllBuffered, name);
        }
    }

    [PunRPC]
    public void RPC_DisplayWinnerName(string name)
    {
        nameDisplay.text = name + " WINS!";

        if (name == PlayerPrefs.GetString("PREFS_PlayerName"))
        {
            messageDisplay.text = "Congratulations!";
        }
        else
        {
            messageDisplay.text = "Better luck next time!";
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //
    }
}
