using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_UIManager : UIManager
{
    bool _showPauseScreen, _showSettingsMenu = false;

    public override void SetUI()
    { /*
        _childPanels[0] : Pause menu
        _childPanels[1] : Settings menu

      */
        _childPanels[0].SetActive(_showPauseScreen);
        _childPanels[1].SetActive(_showSettingsMenu);

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Pause();
    }

    public void Pause()
    {
        _showSettingsMenu = false;
        _showPauseScreen = true;
        Time.timeScale = 0;
        SetUI();
    }
    public void Play()
    {
        _showPauseScreen = false;
        Time.timeScale = 1;
        SetUI();
    }

    public void ShowSettingsMenu()
    {
        _showPauseScreen = false;
        _showSettingsMenu = true;
        SetUI();
    }

}
