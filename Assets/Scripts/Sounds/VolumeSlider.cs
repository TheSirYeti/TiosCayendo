using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider sfxSlider;
    public Slider musicSlider;
    void Start()
    {
        sfxSlider.value = SoundManager.instance.volumeSFX;
        musicSlider.value = SoundManager.instance.volumeMusic;
    }

    public void ChangeSfxVolume()
    {
        SoundManager.instance.ChangeVolumeSound(sfxSlider.value);
    }
    
    public void ChangeMusicVolume()
    {
        SoundManager.instance.ChangeVolumeMusic(musicSlider.value);
    }
}
