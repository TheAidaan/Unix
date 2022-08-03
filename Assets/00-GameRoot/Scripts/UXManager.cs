using UnityEngine;
using Cinemachine;
using UnityEngine.Audio;


public class UXManager : MonoBehaviour
{
    public static UXManager instance;

    CinemachineVirtualCamera[] _cameras = new CinemachineVirtualCamera[2];

    bool _introCamerInView = true;
    //[SerializeField]
    Camera _mainCamera;
    Color _dark = new Color(0.29f, 0.29f, 0.29f, 1);
    Color _light = new Color(1, 1, 1, 1);

    AudioMixer _audio;
    static float _musicVolume, _soundMusic;
    public static float MusicVolume { get { return _musicVolume; } }
    public static float SoundVolume { get { return _soundMusic; } }
    void Awake()
    {
        instance = this;

    }
    // Start is called before the first frame update
    void Start()
    {
        int i = 0;

        foreach (CinemachineVirtualCamera camera in GetComponentsInChildren<CinemachineVirtualCamera>())
        {
            _cameras[i] = camera;
            i++;
        }

        if (GameData.generateBoard)
        {
            transform.position = new Vector3(88f, 87f, 94.9f);
        }

        _mainCamera = GetComponentInChildren<Camera>();

        _audio = Resources.Load<AudioMixer>("Audio");

        _audio.GetFloat("MusicVolume", out _musicVolume);   //get the volume for music
        _audio.GetFloat("SoundVolume", out _soundMusic);    //get the volume for sound
    }

    void SwitchCameras()
    {
        if (_introCamerInView)
        {
            _cameras[0].Priority = 0;
            _cameras[1].Priority = 1;

            _introCamerInView = false;

        }
        else
        {
            _cameras[0].Priority = 1;
            _cameras[1].Priority = 0;


            _introCamerInView = true;

        }
    }

    void DarkMode()
    {
        _mainCamera.backgroundColor = _dark;
    }
    
    void LightMode()
    {
        _mainCamera.backgroundColor = _light;
    }

    /*       STATICS         */  
    public static void Static_SwitchCameras()
    {
        instance.SwitchCameras();
    }
    
    public static void Static_DarkMode()
    {
        instance.DarkMode();
    }
    
    public static void Static_LightMode()
    {
        instance.LightMode();
    }

    void SetVolume(string mixerGroup, float SliderValue)
    {
        _audio.SetFloat(mixerGroup, ConvertToLog(SliderValue));
    }

    float ConvertToLog(float value)
    {
        return Mathf.Log10(value) * 20;
    }

    public static void Static_SetSoundVolume(float value)
    {
        instance.SetVolume("SoundVolume", value);
    }
    public static void Static_SetMusicVolume(float value)
    {
        instance.SetVolume("MusicVolume", value);
    }

}
