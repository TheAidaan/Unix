using UnityEngine;
using System;
using System.Collections;
public class GameManager : MonoBehaviour
{


    public static GameManager instance;

    UnitManager _unitManager;

    static int _blueTeamScore, _redTeamScore;
    public static int blueTeamScore { get { return _blueTeamScore; } }
    public static int redTeamScore { get { return _redTeamScore; } }

    static bool _redTeamWon;
    public static bool redTeamWon { get { return _redTeamWon; } }

    static bool _aiEvaluationInProgress;
    public static bool aiEvaluationInProgress { get { return _aiEvaluationInProgress; } }
    
    static bool _gameInProgress;
    public static bool gameInProgress { get { return _gameInProgress; } }
    
    static bool _unitSelected;
    public static bool unitSelected { get { return _unitSelected; } }

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
        if (Input.GetMouseButtonDown(0) && !_unitSelected && !UIManager.gamePaused)
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

            if (GameData.loadMinMaxScript)
                Destroy(gameObject.GetComponent<MiniMax>());

            if (GameData.loadMachineLearningScript)
            {
                Destroy(gameObject.GetComponent<Brain>());
            }
        }

    }

    void Play()
    {
        StartCoroutine(WaitToPlay());
    } 
    void StartGame()
    {
        _gameOver = false;
        _blueTeamScore = 0;
        _redTeamScore = 0;

        updateUI?.Invoke();

        if (GameData.loadMinMaxScript)
            gameObject.AddComponent<MiniMax>();

        if (GameData.loadMachineLearningScript)
            gameObject.AddComponent<Brain>();

        Board board = GetComponent<Board>();
        _unitManager = GetComponent<UnitManager>();

        board.Create();
        _unitManager.Setup(board);

        SetAIEvaluationStatus(false);
    }

    public void Restart()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        GameData.STATIC_Restart();

        _blueTeamScore = 0;
        _redTeamScore = 0;

        updateUI?.Invoke();
    }
   

    public IEnumerator WaitToPlay()
    {

        yield return new WaitForSeconds(2);


        if (!_gameOver)
        
            play?.Invoke();
     
        else
            play = null;

    }
    public void SetAIEvaluationStatus(bool status)
    {
        _aiEvaluationInProgress = status;
    }

    public void SetGameInProgress(bool status)
    {
        _gameInProgress = status;
    } 
    
    public void SetUnitSelected(bool status)
    {
        _unitSelected = status;
    }

    /*                  STATICS                  */


    public static void STATIC_SetGameInProgress(bool status)
    {
        instance.SetGameInProgress(status);
    }


    public static void STATIC_SetUnitSelected(bool status)
    {
        instance.SetUnitSelected(status);
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
    
    public static void Static_RestartGame()
    {
        instance.Restart();
    } 
    public static void Static_StartGame()
    {
        instance.StartGame();
    }
}
