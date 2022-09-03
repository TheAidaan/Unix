using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class GameManager : MonoBehaviour
{
    static bool _activeGame;
    public static bool activeGame { get { return _activeGame; } }

    public static GameManager instance;

    UnitManager _unitManager;

    static GameData _gameData = new GameData();
    static public GameData gameData { get { return _gameData; } }


    static int _blueTeamScore, _redTeamScore;
    public static int blueTeamScore { get { return _blueTeamScore; } }
    public static int redTeamScore { get { return _redTeamScore; } }

    static bool _redTeamWon;
    public static bool redTeamWon { get { return _redTeamWon; } }

    static bool _aiEvaluationInProgress;
    public static bool aiEvaluationInProgress { get { return _aiEvaluationInProgress; } }

    static bool _gameOver;
    public static bool gameOver { get { return _gameOver; } }

    public static event Action updateUI;
    public static event Action play;
    public static event Action endGame;


    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _activeGame)
        {
            RaycastHit hit;
            Ray ray = UXManager.mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.transform != null)//did the mouse hit anything?
                {
                    if (hit.transform.gameObject.CompareTag("Interactive")) // only interact with interactive gameObjects
                    {
                        BaseUnit clickedUnit = hit.transform.gameObject.GetComponent<BaseUnit>();
                        if (clickedUnit != null)
                        {
                            clickedUnit.Clicked();
                        }
                    }
                }
            }
        }
    }
    public void SwitchSides(Color color)
    {
        _unitManager.SwitchSides(color);
    }

    public void UnitDeath(Color color)
    {
        if (color == Color.red)
            _blueTeamScore += 1;
        else
            _redTeamScore += 1;

        if (_redTeamScore == 12)
        {
            _redTeamWon = true;
            _gameOver = true;
        }
        if (_blueTeamScore == 12)
        {
            _redTeamWon = false;
            _gameOver = true;
        }


        updateUI?.Invoke();
        

        if (_gameOver)
        {
            endGame?.Invoke();
            _activeGame = false;
        }

    }

    void Play()
    {
        StartCoroutine(WaitToPlay());
    } 

    void StartGame( bool activeGame)
    {
        if (activeGame)
        UXManager.Static_SwitchCameras();
        _activeGame = activeGame;

        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
        Board board = GetComponent<Board>();
        board.Create();
        _unitManager = GetComponent<UnitManager>();

        
        _unitManager.Setup(board);

        _blueTeamScore = 0;
        _redTeamScore = 0;

        updateUI?.Invoke();  



        SetAIEvaluationStatus(false);
    }
   

    public IEnumerator WaitToPlay()
    {
        yield return new WaitForSeconds(2);
        if (!_gameOver)
            play?.Invoke();
        else
            play = null;

    }

    void AssignGameData(GameData data)
    {
        if (_gameData.LoadMinMaxScript())
            Destroy(gameObject.GetComponent<MiniMax>());

        if (_gameData.LoadMachineLearningScript())
            Destroy(gameObject.GetComponent<Brain>());

        _gameOver = true;

        endGame?.Invoke();


        if (data.LoadMinMaxScript())
            gameObject.AddComponent<MiniMax>();

        if (data.LoadMachineLearningScript())
            gameObject.AddComponent<Brain>();
        _gameData = data;


    }
    void SetUnitsGameData(List<BaseUnit> units, Color color)
    {
        if (color == Color.red)
            _gameData.SetRedUnits(units);
        else
            _gameData.SetBlueUnits(units);

    }

    #region Statics


    public void SetAIEvaluationStatus(bool status)
    {
        _aiEvaluationInProgress = status;
    }

    public static void STATIC_SetAIEvaluationStatus(bool status)
    {
        instance.SetAIEvaluationStatus(status);
    }

    public static void Static_SwitchSides(Color color, string characterID, string tileID)
    { 
        instance.SwitchSides(color);
    }
    public static void Static_UnitDeath(Color color)
    {
        instance.UnitDeath(color);
    }

    public static void Static_Play()
    {
        instance.Play();
    } 
    public static void Static_StartGame(bool activeGame)
    {
        instance.StartGame(activeGame);
    }

    public static void STATIC_AssignGameData(GameData data)
    {
        instance.AssignGameData(data);
    } 
    
    public static void STATIC_SetUnitsGameData(List<BaseUnit> units, Color color)
    {
        instance.SetUnitsGameData(units, color) ;
    }
#endregion Statics
}
