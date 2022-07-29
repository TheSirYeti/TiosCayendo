using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class MovingDoors : MonoBehaviourPun, IPunObservable
{
    public List<GameObject> objectsToMove;
    private List<Vector3> originalPositions, finalPositions;
    public float distanceToMove = 14f;
    public float timeToMove = 1f;
    public int firstObject;

    private void Awake()
    {
        EventManager.Subscribe("OnRaceStart", StartMovement);
    }

    private void Start()
    {
        originalPositions = new List<Vector3>();
        finalPositions = new List<Vector3>();

        foreach (var obj in objectsToMove)
        {
            originalPositions.Add(obj.transform.position);
            finalPositions.Add(obj.transform.position + new Vector3(0, distanceToMove, 0));
        }
    }

    void StartMovement(object[] parameters)
    {
        photonView.RPC("RPC_StartMovement", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void RPC_StartMovement()
    {
        StartCoroutine(MovementCycle());
    }
    
    IEnumerator MovementCycle()
    {
        int counter = firstObject;

        while (true)
        {
            LeanTween.move(objectsToMove[counter], finalPositions[counter], timeToMove);
            yield return new WaitForSeconds(timeToMove + 1);
            LeanTween.move(objectsToMove[counter], originalPositions[counter], timeToMove);
            yield return new WaitForSeconds(timeToMove + 1);

            counter++;
            if (counter >= objectsToMove.Count) counter = 0;
            yield return new WaitForEndOfFrame();
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //
    }
}
