using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySpecific : MonoBehaviour
{
    public SoundID soundID;
    public MusicID musicID;

    public bool playMusicOnStart;
    public bool playSoundOnStart;
    public bool shouldStopAllMusic;
    public bool shouldStopAllSounds;

    private void Start()
    {
        if (shouldStopAllMusic)
            SoundManager.instance.StopAllMusic();
        
        if (shouldStopAllSounds)
            SoundManager.instance.StopAllSounds();
            
        if(playMusicOnStart)
            SoundManager.instance.PlayMusic(musicID, true);
        
        if(playSoundOnStart)
            SoundManager.instance.PlaySound(soundID);
    }

    public void PlaySpecificMusic(bool shouldLoop)
    {
        if(playMusicOnStart)
            SoundManager.instance.PlayMusic(musicID, shouldLoop);
    }

    public void PlaySpecificSound()
    {
        SoundManager.instance.PlaySound(soundID);
    }
}
