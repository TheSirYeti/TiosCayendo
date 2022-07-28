using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentGameValues : MonoBehaviour
{
    public static CurrentGameValues instance;

    public bool amWinner = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
