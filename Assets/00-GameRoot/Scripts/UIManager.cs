using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;



public class UIManager : UIManager_Base
{
    bool _showTitleScreen = true;
    bool _showSettings, _showGameModes, _showDifficultyOptions, _showAdvancedPlayOptions; //pregame
    bool _showPauseScreen, _showGameOverScreen; //ingame

    [SerializeField]
    TextMeshProUGUI _txtTitle, _txtRedTeamScore, _txtBlueTeamScore, _txtWinner;

    GameData _newGameData;
    private void Start()
    {
        GameData SpectatorBattle = new GameData(8, false, true, true, true,2) ;

        GameManager.STATIC_AssignGameData(SpectatorBattle);

        GameManager.Static_StartGame(false);

        _txtRedTeamScore.transform.parent.gameObject.SetActive(false);
        _txtBlueTeamScore.transform.parent.gameObject.SetActive(false);

        _newGameData = new GameData();
    }

    public override void SetUI()
    { /*
        _childPanels[0] : Title
        _childPanels[1] : Settings
        _childPanels[2] : Game Modes
        _childPanels[3] : Difficulty [scrap]

        _childPanels[4] : Pause
        _childPanels[5] : GameOver

      */

        _childPanels[0].SetActive(_showTitleScreen);
        _childPanels[1].SetActive(_showSettings);
        _childPanels[2].SetActive(_showGameModes); 
        _childPanels[3].SetActive(_showDifficultyOptions);

        _childPanels[4].SetActive(_showPauseScreen);
        _childPanels[5].SetActive(_showGameOverScreen);
    }

    public void ChangeBackground(bool switchToDark)
    {
        if(switchToDark)
            UXManager.Static_DarkMode();
        else
            UXManager.Static_LightMode();

    }

    public void Leave()  
    {

        _showTitleScreen = true;
        _showSettings = false;
        _showGameModes = false;
        _showDifficultyOptions = false;
        _showAdvancedPlayOptions = false;
        SetUI();
    }           //to do

    public void StartGame()
    {

        _txtRedTeamScore.transform.parent.gameObject.SetActive(true);
        _txtBlueTeamScore.transform.parent.gameObject.SetActive(true);
        _txtTitle.gameObject.SetActive(false);

        GameManager.updateUI += UpdateScores;
        GameManager.endGame += EndGame;

        _showGameModes = false;
        _showDifficultyOptions = false;
        _showAdvancedPlayOptions = false;


        SetUI();
        GameManager.Static_StartGame(true);
    }  
    
             //main

    #region GameModes
    public void LoadMultiPlayer()
    {
        StartGame();
    }
    public void LoadSinglePlayer(int depth)
    {
        _newGameData.SetMinMaxDepth(depth);
        _newGameData.LoadMinMaxScript(true);


        StartGame();
    }

    public void ComplexBoard(bool loadComplexBoard)
    {
        if (loadComplexBoard)
            _newGameData.SetBoardLength(10);
        else
            _newGameData.SetBoardLength(8);

        _newGameData.ComplexBoard(loadComplexBoard);


    }

    public void AdvancedPlayer()
    {
        _newGameData.LoadMachineLearningScript(true);

        StartGame();
    }

    #endregion


    public void ShowSettingsMenu()
    {
        _showSettings = true;
        if(_showTitleScreen)
            _showTitleScreen = false;
        
        if(_showPauseScreen)
            _showPauseScreen = false;

        SetUI();
    }       //done

    public void ShowPlayOptions()
    {
        _showGameModes = true;
        _showTitleScreen = false;
        SetUI();
    }


    public void ShowDifficultyOptions()
    {
        _showDifficultyOptions = true;
        _showGameModes = false;
        SetUI();
    }

    public void ShowAdvancedPlayOptions()
    {
        _showAdvancedPlayOptions = true;
        _showGameModes = false;
        SetUI();
    }

    // Fuctions used for game to start Playing

    public void SpectatorPlayer()
    {

        _newGameData.SetMinMaxDepth(2);
        _newGameData.LoadMinMaxScript(true);

        _newGameData.LoadMachineLearningScript(true);

        _newGameData.SetSpectatorBattle(true);

        StartGame();
    }

    #region in-game

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.activeGame)
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


  
}
