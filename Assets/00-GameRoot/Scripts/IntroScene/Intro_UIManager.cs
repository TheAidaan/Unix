using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class Intro_UIManager : UIManager
{
    bool _showTitleScreen = true;
    bool _showSettings, _showPlayOptions, _showDifficultyOptions, _showAdvancedPlayOptions; //pregame
    bool _showPauseScreen, _showGameOverScreen; //ingame
    bool _activeGame;

    [SerializeField]
    TextMeshProUGUI _txtInformation, _txtRedTeamScore, _txtBlueTeamScore, _txtWinner;


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
        _childPanels[0] : Title
        _childPanels[1] : Settings
        _childPanels[2] : Game Modes
        _childPanels[3] : Difficulty [scrap]
        _childPanels[4] : AdvancedOptions [scrap]
        _childPanels[5] : Pause
        _childPanels[6] : GameOver

      */

        _childPanels[0].SetActive(_showTitleScreen);
        _childPanels[1].SetActive(_showSettings);
        _childPanels[2].SetActive(_showPlayOptions); 
        _childPanels[3].SetActive(_showDifficultyOptions);
        _childPanels[4].SetActive(_showAdvancedPlayOptions);

        _childPanels[5].SetActive(_showPauseScreen);
        _childPanels[6].SetActive(_showGameOverScreen);
    }
    public void Leave()
    {
        _showTitleScreen = true;
        _showSettings = false;
        _showPlayOptions = false;
        _showDifficultyOptions = false;
        _showAdvancedPlayOptions = false;
        SetUI();
    }

    void StartGame()
    {
        GameManager.Static_StartGame();
        GameManager.updateUI += UpdateScores;
        GameManager.endGame += EndGame;

        _showDifficultyOptions = false;
        SetUI();
    }
    public void ShowSettingsMenu()
    {
        _showSettings = true;
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
        StartGame();
    }

    public void LoadSinglePlayer(int depth)
    {
        GameData.STATIC_SetMinMaxDepth(depth);
        GameData.STATIC_LoadMinMaxScript(true);


        StartGame();


    }

    public void SpectatorPlayer()
    {
        GameData.STATIC_SetBoardLength(10);
        GameData.STATIC_GenerateBoard(true);

        GameData.STATIC_SetMinMaxDepth(2);
        GameData.STATIC_LoadMinMaxScript(true);

        GameData.STATIC_LoadMachineLearningScript(true);

        GameData.STATIC_SetAIBattle(true);

        StartGame();
    }

    #region in-game

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _activeGame)
            Pause();
    }
    public void Pause()
    {
        _showSettings = false;
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
    void EndGame()
    {
        _showGameOverScreen = true;
        _showPauseScreen = false;
        _showSettings = false;
        SetUI();

        string winningTeam = GameManager.redTeamWon ? "red" : "blue";
        Color vertexColor = GameManager.redTeamWon ? new Color32(210, 95, 64, 255) : new Color32(80, 124, 159, 255);
        _txtWinner.text = winningTeam + " team Won!";
        _txtWinner.color = vertexColor;

        GameManager.endGame -= EndGame;
    }
    void UpdateScores()
    {
        _txtBlueTeamScore.text = GameManager.blueTeamScore.ToString();
        _txtRedTeamScore.text = GameManager.redTeamScore.ToString();
    }
    #endregion


    public void AdvancedPlayer()
    {
        GameData.STATIC_SetBoardLength(10);
        GameData.STATIC_GenerateBoard(true);
        GameData.STATIC_LoadMachineLearningScript(true);


        StartGame();
    }
}
