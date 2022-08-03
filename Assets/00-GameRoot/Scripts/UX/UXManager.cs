using UnityEngine;
using Cinemachine;

public class UXManager : MonoBehaviour
{
    public static UXManager instance;

    CinemachineVirtualCamera[] _cameras = new CinemachineVirtualCamera[2];

    bool _introCamerInView = true;
    //[SerializeField]
    Camera _mainCamera;
    Color _dark = new Color(0.29f, 0.29f, 0.29f, 1);
    Color _light = new Color(1, 1, 1, 1);

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

}
