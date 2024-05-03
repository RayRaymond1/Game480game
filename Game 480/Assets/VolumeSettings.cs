using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;


public class VolumeSettings : MonoBehaviour
{
    private static readonly string FirstPlay = "FirstPlay";
    private static readonly string BackgroundPref = "FirstPlay";
    private static readonly string SoundsEffectsPref = "SoundEffectsPref";
    private int firstPlayInt;
    public Slider backgroundSlider, soundEffectsSlider;
    private float backgroundFloat, soundEffectsFloat;
    public AudioSource backgroundAudio;
    public AudioSource[] soundEffectsAudio;

    


    void Start()
    {
        Debug.Log("backgroundSlider: " + backgroundSlider);
        Debug.Log("soundEffectsSlider: " + soundEffectsSlider);
        Debug.Log("backgroundAudio: " + backgroundAudio);
        Debug.Log("soundEffectsAudio: " + soundEffectsAudio);

        firstPlayInt = PlayerPrefs.GetInt(FirstPlay);
        if (firstPlayInt == 0) {
            backgroundFloat = .25f;
            soundEffectsFloat = 0.75f;
            backgroundSlider.value = backgroundFloat;
            soundEffectsSlider.value = soundEffectsFloat;
            PlayerPrefs.SetFloat(BackgroundPref, backgroundFloat);
            PlayerPrefs.SetFloat(SoundsEffectsPref, soundEffectsFloat);
            PlayerPrefs.SetInt(FirstPlay, -1);


        }
        else { 
            backgroundFloat=PlayerPrefs.GetFloat(BackgroundPref);
            backgroundSlider.value = backgroundFloat;
            soundEffectsFloat=PlayerPrefs.GetFloat(SoundsEffectsPref);
            soundEffectsSlider.value=soundEffectsFloat;
        
        }
        
    }
    public void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat(BackgroundPref, backgroundSlider.value);
        PlayerPrefs.SetFloat(SoundsEffectsPref, soundEffectsSlider.value);
    }

    private void OnApplicationFocus(bool infocus)

    {
        if (!infocus) {
            SaveSoundSettings();
        }
        
    }
    public void UpdateSound() { 
        backgroundAudio.volume = backgroundSlider.value;
        for (int i = 0; i < soundEffectsAudio.Length; i++) {
            soundEffectsAudio[i].volume = soundEffectsSlider.value;
        
        }
    }

}
