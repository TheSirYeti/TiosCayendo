using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatforms : MonoBehaviour
{
    public float rotationTime;
    public bool rotateLeft;
    public GameObject objectToRotate;
    
    private void Start()
    {
        int rotationSign = rotateLeft ? -1 : 1;
        LeanTween.rotateAroundLocal(gameObject, Vector3.up, 360f * rotationSign, rotationTime).setRepeat(-1);
        //StartCoroutine(DoSpinCycle());
    }

    IEnumerator DoSpinCycle()
    {
        int rotationSign = rotateLeft ? -1 : 1;
        
        while (true)
        {
            LeanTween.rotateY(objectToRotate, objectToRotate.transform.eulerAngles.y + (360f * rotationSign), rotationTime);
            yield return new WaitForSeconds(rotationTime);
        }
    }
}
