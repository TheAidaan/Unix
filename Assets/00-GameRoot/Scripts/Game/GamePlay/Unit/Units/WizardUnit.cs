using System.Collections.Generic;
using UnityEngine;

public class WizardUnit : BaseUnit
{ 
    public override void Setup(Color TeamColor, Color32 unitColor, string characterID)
    {
        maxHealth = 15;
        coolDown = 4f;
        base.Setup(TeamColor, unitColor, characterID);

        GetComponent<MeshFilter>().mesh = Resources.Load<Mesh>("Models/Wizard");
        gameObject.AddComponent<BoxCollider>();
    }

    #region Movement
    void CreateTilePath(int flipper) //different to the BaseUnits createTilePath
    {
        int currentX = currentTile.boardPosition.x;
        int currentY = currentTile.boardPosition.z;// z represents the world point, but it also represents the y point in the 2D array. 

        //flipper represents the point where the movement option turns to make the L shape in the various directions
        MatchesStates(currentX - 2, currentY + (1 * flipper));//left
        MatchesStates(currentX - 1, currentY + (2 * flipper));//Upper left
        MatchesStates(currentX + 1, currentY + (2 * flipper));//Upper right
        MatchesStates(currentX + 2, currentY + (1 * flipper));//right
    }

    void MatchesStates(int targetX, int targetY)
    {
        TileState tileState = TileState.None;
        tileState = currentTile.board.ValidateTile(targetX, targetY, this);
        
        if (tileState != TileState.Taken && tileState != TileState.OutOfBounds)
            highlightedTiles.Add(currentTile.board.allTiles[targetX, targetY]);

    }

    public override void CheckPath()
    {
        highlightedTiles.Clear();

        CreateTilePath(1); // top half
        CreateTilePath(-1);//bottom half
    }
    #endregion

    #region Attack
    public override List<BaseUnit> CheckForEnemies(bool checkForReturn) // this unit also checks for eneimies to attack while attacking
    {
        List<BaseUnit> targets = new List<BaseUnit>();

        RaycastHit[] hit = Physics.SphereCastAll(transform.position, 15f, Vector3.down);
        foreach (RaycastHit Hit in hit)
        {
            if(Hit.transform.gameObject.layer != transform.gameObject.layer)
            {
                BaseUnit target = Hit.transform.gameObject.GetComponent<BaseUnit>();
                if (target != null)
                {
                    if (!GameManager.aiEvaluationInProgress && !checkForReturn) // if there is no evaluation in progress and this function is NOT being called for a return value
                    {
                        
                            TransitionToState(attackState); 
                            break;
                    }

                    targets.Add(target);
                }
            }         
        }
        return targets;
    }


    public override void Attack()
    {
        List<char> targetChar = new List<char>();
        List<BaseUnit> targets = CheckForEnemies(true);

        if (targets.Count == 0)
        {
            if (brain != null)
            {
                brain.IncreaseUnitWeight(characterID[1]); // increase weight of unit if another unit has been killed or moved out of way 
                brain = null; //i dont want it to increase after every single kill
            }

            TransitionToState(idleState);
        }

        foreach (BaseUnit target in targets)
        {
            StartCoroutine(target.TakeDamage(4, characterID[1])); //attack       
            targetChar.Add(target.characterID[1]);
        }
        if (brain!=null)
        {
            brain.IncreaseTileWeightWizard(targetChar);
        }
        targets.Clear();

    }
    #endregion

    public override void IdleUpdate()
    {
        CheckForEnemies(false);
    }
}
