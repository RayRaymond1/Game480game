
using UnityEngine;

public class Audiosettings : MonoBehaviour
{

    private static readonly string BackgroundPref = "FirstPlay";
    private static readonly string SoundsEffectsPref = "SoundEffectsPref";
    private float backgroundFloat, soundEffectsFloat;
    public AudioSource backgroundAudio;
    public AudioSource[] soundEffectsAudio;
    // Start is called before the first frame update
    void Awake()
    {
        ContinueSettings();
    }

    private void ContinueSettings() {
        backgroundFloat = PlayerPrefs.GetFloat(BackgroundPref);
        backgroundFloat = PlayerPrefs.GetFloat(SoundsEffectsPref);
        backgroundAudio.volume = backgroundFloat;
        for( int i =0; i < soundEffectsAudio.Length; i++) {
            soundEffectsAudio[i].volume = soundEffectsFloat;
        
        }
    
    }

    // Update is called once per frame
   
}
