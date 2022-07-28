using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NicknameManager : MonoBehaviour
{
    public TextMeshProUGUI nicknameDisplay;
    public TMP_InputField inputField;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("PREFS_PlayerName"))
        {
            PlayerPrefs.SetString("PREFS_PlayerName", "Player");
        }
        else
        {
            nicknameDisplay.text = "Welcome, " + PlayerPrefs.GetString("PREFS_PlayerName") + "!";
        }
    }

    public void UpdateName()
    {
        nicknameDisplay.text = "Welcome, " + inputField.text + "!";
        PlayerPrefs.SetString("PREFS_PlayerName", inputField.text);
    }
}
