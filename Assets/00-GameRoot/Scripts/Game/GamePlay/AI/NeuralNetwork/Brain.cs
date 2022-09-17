using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


[Serializable]
public class UnitWeights
{
    public float meleeWeight;
    public float wizardWeight;
    public float rangedWeight;
}

[Serializable]
public class TileWeights
{
    public float meleeWeight;
    public float wizardWeight;
    public float rangedWeight;
}

[Serializable]
public class Weights
{
    public UnitWeights unitWeights;
    public TileWeights tileWeights;
    public int wins;
    public int losses;
}
public class Brain : AI
{
    string path;
    const string DIRECTORY = "/MachineLearningData/";
    const string FILE_NAME = "Weights.json";

    Weights _weights;

    Network _network;

    Move _lastMove = new Move();//stores the move the brain made last to avoid repitition;

    private void Start()
    {
        _weights = LoadWeights();
        if (_weights == null)
            InitaliseWeights();

        GameManager.endGame += SaveWeights;
    }


    #region SAVING,LOADING AND INITIALIZING DATA
    public Weights LoadWeights() //anyone can call this = anyone can speak
    {
        string filePath = Application.dataPath + DIRECTORY + FILE_NAME;
        Weights weights = new Weights();

        if (File.Exists(filePath)) //is there a text asset?
        {
            string json = File.ReadAllText(filePath);
            weights = JsonUtility.FromJson<Weights>(json); // checking if the file should be loaded into a graph or a linked list      
            return weights;

        }
        else
        {
            Debug.Log("No json file found");
            return null;
        }

    }

    public void SaveWeights()
    { 
        if (GameManager.redTeamWon && teamColor == Color.red)
        {
            _weights.wins++;
        }
        else
        {
            _weights.losses++;
        }
        path = Application.dataPath;
        string dir = path + DIRECTORY;
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        string json = JsonUtility.ToJson(_weights);
        File.WriteAllText(dir + FILE_NAME, json);

        Debug.Log("Weights have been saved!");
    }

    void InitaliseWeights()
    {
        _weights = new Weights();
        _weights.tileWeights = new TileWeights();
        _weights.unitWeights = new UnitWeights();

        _weights.tileWeights.meleeWeight = UnityEngine.Random.Range(-0.5f, 0.5f);
        _weights.unitWeights.meleeWeight = UnityEngine.Random.Range(-0.5f, 0.5f);

        _weights.tileWeights.rangedWeight = UnityEngine.Random.Range(-0.5f, 0.5f);
        _weights.unitWeights.rangedWeight = UnityEngine.Random.Range(-0.5f, 0.5f);

        _weights.tileWeights.wizardWeight = UnityEngine.Random.Range(-0.5f, 0.5f);
        _weights.unitWeights.wizardWeight = UnityEngine.Random.Range(-0.5f, 0.5f);

        _weights.losses = 0;
        _weights.wins = 0;
    }

    #endregion


    public override void AssignUnits()
    {  

        if (GameData.minMaxColor == Color.white)
        {
            base.AssignUnits();
        }
        else
        {
            aiUnits = GameData.minMaxColor == Color.red ?  GameData.blueUnits : GameData.redUnits;
            teamColor = GameData.minMaxColor == Color.red ? Color.blue : Color.red;
        }
        string color = teamColor == Color.red ? "red" : "blue";
        Debug.Log("Neural Network is playing as the " + color + " team");
        GameData.STATIC_SetGeneticAIColor(teamColor);
    }


    public override void Play()
    {
        _network = new Network(FilteredUnits(), _weights);

        Move move = _network.output[0].move;

        move.unit.SetBrain(this);
        move.unit.Move(move.target);

        _lastMove = move;
        

        base.Play();

    }

    List<BaseUnit> FilteredUnits() // loops through all units to produce only the units that can move
    {
        List<BaseUnit> filteredUnits = new List<BaseUnit>();
        foreach (BaseUnit unit in aiUnits)
        {
            if (CheckValidMoves(unit) != null)
            {
                if(unit != _lastMove.unit)
                {
                    filteredUnits.Add(unit);

                }

            }
        }

        if(filteredUnits.Count == 0 && !GameManager.gameOver)
        {
            filteredUnits.Add(_lastMove.unit);
        }
        return filteredUnits;
    }

    void RandomMover()
    {

        BaseUnit unit = aiUnits[UnityEngine.Random.Range(0, aiUnits.Count)];

        if (!unit.gameObject.activeSelf)
            RandomMover();
        else
        {
            unit.CheckPath();
            if (unit.highlightedTiles.Count == 0)
                RandomMover();
            else
                unit.Move(unit.highlightedTiles[UnityEngine.Random.Range(0, unit.highlightedTiles.Count)]);
        }

    }


    //Weight Effectors


    public void DecreaseUnitWeight(char character)
    {
        switch (character)
        {
            case 'R':
                _weights.unitWeights.rangedWeight -= (float)Math.Tanh((int)character);
            break;
            case 'M':
                _weights.unitWeights.meleeWeight -= (float)Math.Tanh((int)character);
            break;
            case 'W':
                _weights.unitWeights.wizardWeight -= (float)Math.Tanh((int)character);
            break;
        }
    }

    public void IncreaseUnitWeight(char character)
    {
        switch (character)
        {
            case 'R':
                _weights.unitWeights.rangedWeight += (float)Math.Tanh((int)character);
                break;
            case 'M':
                _weights.unitWeights.meleeWeight += (float)Math.Tanh((int)character);
                break;
            case 'W':
                _weights.unitWeights.wizardWeight += (float)Math.Tanh((int)character);
                break;
        }
    }

    public void IncreaseTileWeightSimple(char character,char target)
    {
        switch (character)
        {
            case 'R':
                _weights.tileWeights.rangedWeight += (float)Math.Tanh((int)target);
                break;
            case 'M':
                _weights.tileWeights.meleeWeight += (float)Math.Tanh((int)target);
                break;
        }
    }

    public void IncreaseTileWeightWizard(List<char> targets)
    {
       int total=0;

       foreach(Char c in targets)
       {
            total += (int)c;
       }

        _weights.tileWeights.wizardWeight += (float)Math.Tanh(total);

    }

    public void DecreaseTileWeights(char character, char attacker)
    {

        switch (character)
        {
            case 'R':
                _weights.tileWeights.rangedWeight -= (float)Math.Tanh(attacker);
                break;
            case 'M':
                _weights.tileWeights.meleeWeight -= (float)Math.Tanh(attacker);
                break;
            case 'W':
                _weights.tileWeights.wizardWeight -= (float)Math.Tanh(attacker);
                break;
        }

    }
}

