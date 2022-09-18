using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;



public class UIManager : UIManager_Base
{
    bool _showTitleScreen = true;
    bool _showSettings, _showGameModes, _showDifficultyOptions, _showAdvancedPlayOptions; //pregame
    bool _showPauseScreen, _showGameOverScreen; //ingame

    [SerializeField]
    TextMeshProUGUI _txtTitle, _txtRedTeamScore, _txtBlueTeamScore, _txtWinner;


    [SerializeField]
    GameData data;

    IntroAnimationController _IntroAnimation;

    public static event Action changeTextColor;

    private void Start()
    {
        //StartIntroAnimtaion();


        GameData.STATIC_SetBoardLength(8);
        GameData.STATIC_LoadMinMaxScript(false);
        GameData.STATIC_GenerateBoard(false);
        GameData.STATIC_SetAIBattle(false);
        GameData.STATIC_LoadMachineLearningScript(false);
        GameData.STATIC_SetMinMaxColor(Color.white);
        GameData.STATIC_SetGeneticAIColor(Color.white);
        GameData.STATIC_SetPlayerColor(Color.white);
        _txtRedTeamScore.transform.parent.gameObject.SetActive(false);
        _txtBlueTeamScore.transform.parent.gameObject.SetActive(false);

       
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
        _childPanels[2].SetActive(_showGameModes); 
        _childPanels[3].SetActive(_showDifficultyOptions);
        _childPanels[4].SetActive(_showAdvancedPlayOptions);

        _childPanels[5].SetActive(_showPauseScreen);
        _childPanels[6].SetActive(_showGameOverScreen);
    }

    public void ChangeBackground(bool switchToDark)
    {
        if(switchToDark)
            UXManager.Static_DarkMode();
        else
            UXManager.Static_LightMode();

        changeTextColor?.Invoke();


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

    #region pre-Game

    void StartIntroAnimtaion()
    {
        GameObject simulation = Resources.Load<GameObject>("Prefabs/IntroAnimation");
        Instantiate(simulation, new Vector3(120, 0, 60), Quaternion.Euler(new Vector3(0, 90, 0)));

        _IntroAnimation = simulation.GetComponent<IntroAnimationController>();

    }

    public void ShowSettingsMenu()
    {
        _showSettings = true;
        if (_showTitleScreen)
            _showTitleScreen = false;

        if (_showPauseScreen)
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

    public IEnumerator StartGame()
    {
        UXManager.Static_SwitchCameras();
        yield return new WaitForSeconds(0.3f);

        _txtRedTeamScore.transform.parent.gameObject.SetActive(true);
        _txtBlueTeamScore.transform.parent.gameObject.SetActive(true);
        _txtTitle.gameObject.SetActive(false);

        GameManager.updateUI += UpdateScores;
        GameManager.endGame += EndGame;

        _showGameModes = false;
        _showDifficultyOptions = false;
        _showAdvancedPlayOptions = false;


        SetUI();
        GameManager.Static_StartGame();
    }

    #region GameModes
    public void LoadMultiPlayer()
    {
        StartCoroutine(StartGame());
    }
    public void LoadSinglePlayer(int depth)
    {
        GameData.STATIC_SetMinMaxDepth(depth);
        GameData.STATIC_LoadMinMaxScript(true);


        StartCoroutine(StartGame());

    }

    public void ComplexBoard(bool loadComplexBoard)
    {
        if (loadComplexBoard)
            GameData.STATIC_SetBoardLength(10);
        else
            GameData.STATIC_SetBoardLength(8);

        GameData.STATIC_GenerateBoard(loadComplexBoard);

    }

    public void AdvancedPlayer()
    {
        GameData.STATIC_LoadMachineLearningScript(true);
        GameData.STATIC_LoadMinMaxScript(false);



        StartCoroutine(StartGame());

    }

    #endregion
    #endregion

    #region Game

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
   
    void UpdateScores()
    {
        _txtBlueTeamScore.text = GameManager.blueTeamScore.ToString();
        _txtRedTeamScore.text = GameManager.redTeamScore.ToString();
    }
    #endregion

    #region post-Game
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
    #endregion

    #region Testings

    public void SpectatorPlayer()
    {
        GameData.STATIC_SetBoardLength(10);
        GameData.STATIC_GenerateBoard(true);

        GameData.STATIC_SetMinMaxDepth(2);
        GameData.STATIC_LoadMinMaxScript(true);

        GameData.STATIC_LoadMachineLearningScript(true);

        GameData.STATIC_SetAIBattle(true);

        StartCoroutine(StartGame());

    }

    #endregion

}
