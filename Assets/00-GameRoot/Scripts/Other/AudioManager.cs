using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    AudioMixer _audio;
    static float _musicVolume, _soundMusic;
    public static float MusicVolume { get { return _musicVolume; } }
    public static float SoundVolume { get { return _soundMusic; } }
    void Awake()
    {
        instance = this;

        _audio = Resources.Load<AudioMixer>("Audio");        

        _audio.GetFloat("MusicVolume", out _musicVolume);   //get the volume for music
        _audio.GetFloat("SoundVolume", out _soundMusic);    //get the volume for sound
    }
    public void SetVolume(string mixerGroup, float SliderValue)
    {
        _audio.SetFloat(mixerGroup, ConvertToLog(SliderValue));
    }

    float ConvertToLog(float value)
    {
        return Mathf.Log10(value)*20;
    }

    /*              PUBLIC STATICS              */

    public static void Static_SetSoundVolume(float value)
    {
        instance.SetVolume("SoundVolume", value);
    }
    public static void Static_SetMusicVolume(float value)
    {
        instance.SetVolume("MusicVolume",value);
    }
}
