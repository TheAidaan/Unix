using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Move
{
    public BaseUnit unit;
    public Tile target;

    public Move(BaseUnit Unit, Tile Target)
    {
        unit = Unit;
        target = Target;
    }
}

public abstract class AI : MonoBehaviour
{
    protected List<BaseUnit> otherUnits, aiUnits;
    protected Color teamColor;
    public Move bestMove;

    public Dictionary<char, double> evaluationScoreLibrary = new Dictionary<char, double>()
    {
        {'M', 1 },
        {'R', 2 },
        {'W', 3 },

    };
    public virtual void AssignUnits()
    {

        int rand = Random.Range(0, 2);
        


        otherUnits = rand == 0 ? GameData.blueUnits : GameData.redUnits;
        aiUnits = rand == 0 ? GameData.redUnits : GameData.blueUnits;
        teamColor = rand == 0 ? Color.red : Color.blue;

        if (!GameData.aiBattle)
        {
            Color playerColor = teamColor == Color.red ? Color.blue : Color.red;
            GameData.STATIC_SetPlayerColor(playerColor);
        }

    }

    protected List<Tile> CheckValidMoves(BaseUnit unit)
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

        int perspective = unit.teamColor == teamColor ? 1 : -1; //the AI wants a high evaluation for itself and a low evaluation fot the other;
        evaluation *= perspective;

        return evaluation;

    }

    public virtual void Play() { GameManager.play -= Play; }
}
