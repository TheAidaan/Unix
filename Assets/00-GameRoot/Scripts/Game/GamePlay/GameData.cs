using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    
    public GameData() { }
    public GameData(int boardLength, bool complexBoard, bool loadMinMaxScript, bool loadMachineLearningScript, bool spectatorBattle, int minMaxDepth)
    {
        _boardLength = boardLength;
        _complexBoard = complexBoard;

        _loadMinMaxScript = loadMinMaxScript;
        _loadMachineLearningScript = loadMachineLearningScript;
        _spectatorBattle = spectatorBattle;

        _minMaxDepth = minMaxDepth;
    }

    #region AI

   int  _boardLength;
    public void SetBoardLength(int Length)
    {
        _boardLength = Length;
    }

    public int GetBoardLength()
    {
        return _boardLength;
    }

     
    bool _loadMinMaxScript, _loadMachineLearningScript, _complexBoard, _spectatorBattle;


    #region MiniMax

    public bool LoadMachineLearningScript() { return _loadMachineLearningScript; }
    public bool LoadMachineLearningScript(bool loadMachineLearningScript) { return _loadMachineLearningScript= loadMachineLearningScript; }
    public bool LoadMinMaxScript() { return _loadMinMaxScript; }
    public void LoadMinMaxScript(bool loadMinMaxScript) { _loadMinMaxScript = loadMinMaxScript; }
    public void ComplexBoard(bool complexBoard) { _complexBoard = complexBoard; }
    public bool ComplexBoard() { return _complexBoard; }
    public bool SpectatorBattle() { return _spectatorBattle; }
    public void SetSpectatorBattle( bool spectatorBattle) { _spectatorBattle = spectatorBattle; }

    int _minMaxDepth;
    public void SetMinMaxDepth(int depth)
    {
        _minMaxDepth = depth;
    }   
    public int GetMinMaxDepth()
    {
       return _minMaxDepth;
    }
    Color _minMaxColor = Color.white;
    public void SetMinMaxColor(Color color)
    {
        _geneticAIColor = color;
    }
    public Color GetMinMaxColor()
    {
        return _geneticAIColor;
    }
    Color _playerColor= Color.white;
    Color _geneticAIColor = Color.white;
    public Color GetGeneticAIColor() { return _geneticAIColor; }
    public Color GetPlayerColor() { return _playerColor; }

    public void SetGeneticAIColor(Color color)
    {
        _geneticAIColor = color;
    }
    public void SetPlayerColor(Color color)
    {
        _playerColor = color;
    }


    #endregion MiniMax

    #endregion AI

    #region Units


    List<BaseUnit> _redUnits, _blueUnits;

    public List<BaseUnit> GetRedUnits()
    {
        return _redUnits;
    }
    public List<BaseUnit> GetBlueUnits()
    {
        return _blueUnits;
    }
    public void SetBlueUnits(List<BaseUnit> units)
    {
        _blueUnits = units;
    }
    public void SetRedUnits(List<BaseUnit> units)
    {

        _redUnits = units;
    }


#endregion Units

}