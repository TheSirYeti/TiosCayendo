using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleColliders : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Renderer>().enabled = false;
    }
}
