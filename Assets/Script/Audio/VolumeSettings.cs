using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider VFXSlider;

    void Start(){
        if(PlayerPrefs.HasKey("MusicVolume")){
            LoadVolumeValue();
        }else{
            SetVolumeMusic();
        }
    }

    public void SetVolumeMusic(){
        float volume = musicSlider.value;
        float volume2 = VFXSlider.value;
        audioMixer.SetFloat("Music", Mathf.Log10(volume)*20);
        audioMixer.SetFloat("VFX", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.SetFloat("VFXVolume", volume2);
    }

    private void LoadVolumeValue(){
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        VFXSlider.value = PlayerPrefs.GetFloat("VFXVolume");

        SetVolumeMusic();
    }
}
