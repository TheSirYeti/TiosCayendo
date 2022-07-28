using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public GameObject prefabName;
    private void Start()
    {
        PhotonNetwork.Instantiate(prefabName.name, SpawnPositions.instance.positions[PhotonNetwork.LocalPlayer.ActorNumber - 1].position,
            Quaternion.identity);
    }
}
