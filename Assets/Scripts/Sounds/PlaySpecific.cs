using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySpecific : MonoBehaviour
{
    public SoundID soundID;

    public void PlaySpecificSound()
    {
        SoundManager.instance.PlaySound(soundID);
    }
}
