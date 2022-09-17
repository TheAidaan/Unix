using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameData : SingletonScriptableObject<GameData>
{
    
    public static int boardLength { get { return _boardLength; } }

    //AI

    static bool _loadMinMaxScript, _loadMachineLearningScript, _generateBoard,_aiBattle;
    public static bool loadMinMaxScript { get { return _loadMinMaxScript; } }

    static int _minMaxDepth, _boardLength;
    public static int minMaxDepth { get { return _minMaxDepth; } }
    public static bool loadMachineLearningScript { get { return _loadMachineLearningScript; } }
    public static bool generateBoard { get { return _generateBoard; } }
    public static bool aiBattle { get { return _aiBattle; } }

    //Units

    static List<BaseUnit> _redUnits;
    public static List<BaseUnit> redUnits { get { return _redUnits; } }

    static List<BaseUnit> _blueUnits;
    public static List<BaseUnit> blueUnits { get { return _blueUnits; } }

    //Color

    static Color _minMaxColor, _geneticAIColor, _playerColor;
    public static Color minMaxColor { get { return _minMaxColor; } }
    public static Color geneticAIColor { get { return _geneticAIColor; } }
    public static Color playerColor { get { return _playerColor; } }

    void SetBoardLength(int Length)
    {
        _boardLength = Length;
    }
    //AI
        //MinMax
    void LoadMinMaxScript(bool load)
    {
        _loadMinMaxScript = load;
    }

    void SetMinMaxDepth(int depth)
    {
        _minMaxDepth = depth;
    }
    //loadMachineLearningScript

    void LoadMachineLearningScript(bool load)
    {
        _loadMachineLearningScript = load;
    }

            //Board
    void GenerateBoard(bool generate)
    {
        _generateBoard = generate;

        _boardLength = _generateBoard ? 10 : 8;
    }


    void SetAIBattle(bool battle)
    {
        _aiBattle = battle;

    }



    //Lists
    void SetBlueUnits(List<BaseUnit> units)
    {
        _blueUnits = units;
    }
    void SetRedUnits(List<BaseUnit> units)
    {
        
        _redUnits = units;
    }

    //Colors
    void SetMinMaxColor(Color color)
    {
        _minMaxColor = color;
    }
    void SetGeneticAIColor(Color color)
    {
        _geneticAIColor = color;
    }
    void SetPlayerColor(Color color)
    {
        _playerColor = color;
    } 

    /*                  STATICS                  */

    //Colors
    public static void STATIC_SetMinMaxColor(Color color)
    {
        instance.SetMinMaxColor(color);
    }
    public static void STATIC_SetGeneticAIColor(Color color)
    {
        instance.SetGeneticAIColor(color);
    }  
    
    public static void STATIC_SetPlayerColor(Color color)
    {
        instance.SetPlayerColor(color);
    }

    public static void STATIC_SetBlueUnits(List<BaseUnit> units)
    {
        instance.SetBlueUnits(units);
    }
    public static void STATIC_SetRedUnits(List<BaseUnit> units)
    {
        instance.SetRedUnits(units);
    }
    public static void STATIC_SetBoardLength(int length)
    {
        instance.SetBoardLength(length);
    }

    // MiniMax

    public static void STATIC_SetMinMaxDepth(int depth)
    {
        instance.SetMinMaxDepth(depth);
    } 

    public static void STATIC_LoadMinMaxScript(bool load)
    {
        instance.LoadMinMaxScript(load);
    }   

    //Genetic AI
    
    public static void STATIC_LoadMachineLearningScript(bool load)
    {
        instance.LoadMachineLearningScript(load);
    }

    //TASK 3

    public static void STATIC_GenerateBoard(bool generate)
    {
        instance.GenerateBoard(generate);
    }

    public static void STATIC_SetAIBattle(bool battle)
    {
        instance.SetAIBattle(battle);
    }

    //DATA
}
