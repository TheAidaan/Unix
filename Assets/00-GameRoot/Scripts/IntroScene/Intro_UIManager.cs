using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro_UIManager : UIManager
{
    bool _showTitleScreen = true;
    bool _showSettingScreen, _showPlayOptions, _showDifficultyOptions, _showAdvancedPlayOptions;
    [SerializeField]
    GameData data;
    private void Start()
    {
        GameData.STATIC_SetBoardLength(8);
        GameData.STATIC_LoadMinMaxScript(false);
        GameData.STATIC_GenerateBoard(false);
        GameData.STATIC_SetAIBattle(false);
        GameData.STATIC_LoadMachineLearningScript(false);
        GameData.STATIC_SetMinMaxColor(Color.white);
        GameData.STATIC_SetGeneticAIColor(Color.white);
        GameData.STATIC_SetPlayerColor(Color.white);


    }

    public override void SetUI()
    { /*
        _childPanels[0] : Title Screen
        _childPanels[1] : Settings Screen
        _childPanels[2] : Settings Screen

      */

        _childPanels[0].SetActive(_showTitleScreen);
        _childPanels[1].SetActive(_showSettingScreen);
        _childPanels[2].SetActive(_showPlayOptions); 
        _childPanels[3].SetActive(_showDifficultyOptions);
        _childPanels[4].SetActive(_showAdvancedPlayOptions);
    }
    public void Leave()
    {
        _showTitleScreen = true;
        _showSettingScreen = false;
        _showPlayOptions = false;
        _showDifficultyOptions = false;
        _showAdvancedPlayOptions = false;
        SetUI();
    }

    public void ShowSettingsMenu()
    {
        _showSettingScreen = true;
        _showTitleScreen = false;
        SetUI();
    }
    public void ShowPlayOptions()
    {
        _showPlayOptions = true;
        _showTitleScreen = false;
        SetUI();
    }


    public void ShowDifficultyOptions()
    {
        _showDifficultyOptions = true;
        _showPlayOptions = false;
        SetUI();
    }

    public void ShowAdvancedPlayOptions()
    {
        _showAdvancedPlayOptions = true;
        _showPlayOptions = false;
        SetUI();
    }

    // Fuctions used for game to start Playing

    public void LoadMultiPlayer()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadSinglePlayer(int depth)
    {
        GameData.STATIC_SetMinMaxDepth(depth);
        GameData.STATIC_LoadMinMaxScript(true);


        SceneManager.LoadScene(1);


    }

    public void SpectatorPlayer()
    {
        GameData.STATIC_SetBoardLength(10);
        GameData.STATIC_GenerateBoard(true);

        GameData.STATIC_SetMinMaxDepth(2);
        GameData.STATIC_LoadMinMaxScript(true);

        GameData.STATIC_LoadMachineLearningScript(true);

        GameData.STATIC_SetAIBattle(true);

        SceneManager.LoadScene(1);
    }

    public void AdvancedPlayer()
    {
        GameData.STATIC_SetBoardLength(10);
        GameData.STATIC_GenerateBoard(true);
        GameData.STATIC_LoadMachineLearningScript(true);


        SceneManager.LoadScene(1);
    }
}
