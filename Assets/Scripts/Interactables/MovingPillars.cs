using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPillars : MonoBehaviour
{
    public float movementTime;
    public Transform waypoint1, waypoint2;

    private void Awake()
    {
        EventManager.Subscribe("OnRaceStart", StartMovingPillars);
    }


    public void StartMovingPillars(object[] parameters)
    {
        StartCoroutine(DoMovingCycle());
    }

    IEnumerator DoMovingCycle()
    {
        while (true)
        {
            LeanTween.move(gameObject, waypoint1.position, movementTime);
            yield return new WaitForSeconds(movementTime);
            LeanTween.move(gameObject, waypoint2.position, movementTime);
            yield return new WaitForSeconds(movementTime);
        }
    }
}
