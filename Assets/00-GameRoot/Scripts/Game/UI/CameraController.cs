using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    bool _introCamerInView;
    [SerializeField]
    CinemachineVirtualCamera[] _cameras = new CinemachineVirtualCamera[2];
    // Start is called before the first frame update
    void Awake()
    {
        _introCamerInView = true;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchView();
        }
    }
    void SwitchView()
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
}
