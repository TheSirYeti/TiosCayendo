using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class GetWinner : MonoBehaviourPun, IPunObservable
{
    public TextMeshProUGUI nameDisplay;
    public TextMeshProUGUI messageDisplay;
    public List<GameObject> allDanceEmotes;

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
            int rand = UnityEngine.Random.Range(0, allDanceEmotes.Count);
            photonView.RPC("RPC_DisplayWinnerName", RpcTarget.AllBuffered, name, rand);
        }
    }

    [PunRPC]
    public void RPC_DisplayWinnerName(string name, int danceID)
    {
        nameDisplay.text = name + " WINS!";
        allDanceEmotes[danceID].SetActive(true);
        
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
