using System;
using System.Collections.Generic;
using UnityEngine;

struct UnitRating
{
    public double rating;
    public BaseUnit unit;

    public TileRating[] moves;
}

struct TileRating
{
    public double rating;
    public Tile tile;
}

public struct BestMove
{
    public double evaluation;
    public Move move;
}

public class Network
{
    public BestMove[] output;
    UnitRating[] _neurons;
    double _bias;

    Dictionary<char, double> _unitWeights = new Dictionary<char, double>();
    Dictionary<char, double> _tileWeights = new Dictionary<char, double>();

    public Network(List<BaseUnit> units, Weights weights)
    {
        _neurons = new UnitRating[units.Count];

        _unitWeights.Add('M', weights.unitWeights.meleeWeight);
        _tileWeights.Add('M', weights.tileWeights.meleeWeight);

        _unitWeights.Add('R', weights.unitWeights.rangedWeight);
        _tileWeights.Add('R', weights.tileWeights.rangedWeight);

        _unitWeights.Add('W', weights.unitWeights.wizardWeight);
        _tileWeights.Add('W', weights.tileWeights.wizardWeight);

        int totalGames = weights.wins + weights.losses;
        _bias = totalGames==0? UnityEngine.Random.Range(-0.5f, 0.5f) : (double)weights.wins / (double)totalGames; //prevents dividing by zero,thus making me sad

        for (int i = 0; i < _neurons.Length; i++)
        {
            _neurons[i].unit = units[i];
        }

        ProcessUnits();
    }

   void ProcessUnits()
   {
        for (int i = 0; i < _neurons.Length; i++)
        {
            _neurons[i].rating = Math.Tanh((double)_neurons[i].unit.characterID[1] *_unitWeights[_neurons[i].unit.characterID[1]]);
        }

        ProcessUnitTiles();
   }

    void ProcessUnitTiles()
    {
        GameManager.STATIC_SetAIEvaluationStatus(true);

        for (int i = 0; i < _neurons.Length; i++)
        {
            List<Tile> validMoves = CheckValidMoves(_neurons[i].unit);
            _neurons[i].moves = new TileRating[validMoves.Count];

            Tile unitCurrentTile = _neurons[i].unit.currentTile;

            for (int j = 0; j < _neurons[i].moves.Length; j++)
            {
                _neurons[i].moves[j].tile = validMoves[j];
                _neurons[i].unit.Move(validMoves[j]);
                
                _neurons[i].moves[j].rating = Math.Tanh(Evaluate(_neurons[i].unit) * _tileWeights[_neurons[i].unit.characterID[1]]);
            }

            _neurons[i].unit.Move(unitCurrentTile);

        }

        GameManager.STATIC_SetAIEvaluationStatus(false);


        PopulateOutputs();
    }

    void PopulateOutputs()
    {
        List<BestMove> allmoves = new List<BestMove>();

        for (int i = 0; i < _neurons.Length; i++)
        {
            for (int j = 0; j < _neurons[i].moves.Length; j++)
            {
                Move move = new Move();

                move.unit = _neurons[i].unit;
                move.target = _neurons[i].moves[j].tile;

                BestMove bestMove = new BestMove();
                bestMove.move = move;

                bestMove.evaluation = _neurons[i].rating + _neurons[i].moves[j].rating +_bias;

                allmoves.Add(bestMove);
            }
        }
        
        output = allmoves.ToArray();

        SortOutputs();
    }

    void SortOutputs()
    {
        for (int i = 0; i < output.Length; i++)
        {
            for (int j = i + 1; j < output.Length; j++)
            {
                if (output[i].evaluation > output[j].evaluation)
                {
                    BestMove temp = output[i];
                    output[i] = output[j];
                    output[j] = temp;
                }
            }
        }
    }

    public Dictionary<char, double> evaluationScoreLibrary = new Dictionary<char, double>()
    {
        {'M', 1 },
        {'R', 2 },
        {'W', 3 },

    };

    List<Tile> CheckValidMoves(BaseUnit unit)
    {

        if (!unit.gameObject.activeSelf)
            return null;

        unit.CheckPath();

        if (unit.highlightedTiles.Count == 0)
            return null;

        return unit.highlightedTiles;
    }

    protected double Evaluate(BaseUnit unit)
    {
        char characterCode;
        BaseUnit singletarget = null;
        List<BaseUnit> targets = new List<BaseUnit>();

        double evaluation = 0;
        characterCode = unit.characterID[1];

        bool simpleEval = characterCode == 'M' || characterCode == 'R' ? true : false;


        if (simpleEval)
        {
            singletarget = unit.CheckForEnemy();
            if (singletarget != null)
                evaluation += evaluationScoreLibrary[characterCode];
        }
        else
        {
            targets = unit.CheckForEnemies(true);

            foreach (BaseUnit target in targets)
            {
                characterCode = target.characterID[1];
                evaluation += evaluationScoreLibrary[characterCode];

            }

        }

        return evaluation;

    }
}
