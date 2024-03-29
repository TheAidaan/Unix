using System.Collections.Generic;
using UnityEngine;

public class WizardUnit : BaseUnit
{

    public override void Setup(Color TeamColor, Color32 unitColor, string characterID)
    {
        maxHealth = 15;
        coolDown = 4f;
        attackLimit = 3;
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
            moveableTiles.Add(currentTile.board.allTiles[targetX, targetY]);

    }

    public override void CheckPath()
    {
        moveableTiles.Clear();

        CreateTilePath(1); // top half
        CreateTilePath(-1);//bottom half
    }
    #endregion

    #region Attack
    public override List<BaseUnit> CheckForEnemies(bool checkForReturn) // this unit also checks for eneimies to attack while attacking
    {
        List<BaseUnit> _targets = new List<BaseUnit>();

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
                            TransitionToState(attackState);  //if in idle it will search for enemies, if atleast one is found it will transition to attack and gather all the enemies it can attack
                            break;
                    }
                    bool invalidTarget = false; //innocent until proven guilty 

                    if (!GameManager.aiEvaluationInProgress)
                    {
                        foreach (TargetTracker targetCheck in invalidTargets)
                        {
                            if (targetCheck.id == target.characterID && targetCheck.tile == target.currentTile.tileID && targetCheck.attackCount == attackLimit)
                                invalidTarget = true;

                        }

                        if (invalidTarget)
                            continue;
                        else
                        {
                            TargetTracker newtarget = new TargetTracker(target.characterID, target.currentTile.tileID);
                            invalidTargets.Add(newtarget);
                            _targets.Add(target);

                        }

                    }
                    else
                    {
                        _targets.Add(target); //a theoretical evaluation should not include the invalid check because if the unit is moved the invalid units lst will be cleared.

                    }
                }
            }         
        }
        return _targets;
    }


    public override void Attack()
    {
        List<char> targetChar = new List<char>();
        targets = CheckForEnemies(true);

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
            if (target.isActiveAndEnabled)
            {
                StartCoroutine(target.TakeDamage(2, characterID[1])); //attack
                foreach (TargetTracker targetCheck in invalidTargets)
                {
                    if (targetCheck.id == target.characterID && targetCheck.tile == target.currentTile.tileID)
                        targetCheck.attackCount++;
                }

                    targetChar.Add(target.characterID[1]);
            }
            
        }
        if (brain!=null)
        {
            brain.IncreaseTileWeightWizard(targetChar);
        }
        targets.Clear();

    }

    public override void Die() {
        if (targets.Count != 0 && gameObject != null)
        {
            foreach (BaseUnit target in targets)
            {
                 target.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");

            }
        }
        HideHighlightedTiles();

        gameObject.SetActive(false);
    }
    #endregion

    public override void IdleUpdate()
    {
        CheckForEnemies(false);
    }
}
