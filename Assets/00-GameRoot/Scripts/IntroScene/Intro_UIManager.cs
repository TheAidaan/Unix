using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro_UIManager : UIManager
{
    bool _showTitleScreen = true;
    bool _showSettingScreen = false;

    public override void SetUI()
    { /*
        _childPanels[0] : Title Screen
        _childPanels[1] : Settings Screen

      */

        _childPanels[0].SetActive(_showTitleScreen);
        _childPanels[1].SetActive(_showSettingScreen);
    }
    public void ShowTitleScreen()
    {
        _showTitleScreen = true;
        _showSettingScreen = false;
        SetUI();
    }

    public void ShowSettingsMenu()
    {
        _showSettingScreen = true;
        _showTitleScreen = false;
        SetUI();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
