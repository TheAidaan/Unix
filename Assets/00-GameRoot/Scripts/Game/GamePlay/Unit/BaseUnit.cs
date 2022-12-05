using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUnit : MonoBehaviour
{
    public Brain brain;
    public int maxHealth;
    public int _health;
    public string characterID;
    public int damage;

    #region Unit setup
    public Color teamColor = Color.clear;
    public Vector3 rotation;

    AudioSource _audioSource;

    public virtual void Setup(Color TeamColor, Color32 unitColor, string CharacterID)
    {
        characterID = CharacterID;
        teamColor = TeamColor;
        attackCount = 0;

        int layer = teamColor == Color.red ? 3:6;
        gameObject.layer = layer;
        GetComponent<Renderer>().material.color = unitColor;

        _health = maxHealth;

        rotation = transform.rotation.eulerAngles;

        _audioSource = GetComponent<AudioSource>();

        _rend = gameObject.GetComponent<Renderer>();

        targets = new List<BaseUnit>();

    }


    #endregion

    #region Unit Movement
    //Movement Variables
    public Vector3Int movement = Vector3Int.one;
    public List<Tile> moveableTiles = new List<Tile>();
    public Tile currentTile = null;
    Tile previousTile = null;

    public bool selected;
    public void CreateTilePath(int xDirection, int yDirection, int movement)
    {
        
        //TargetPosition
        int currentX = currentTile.boardPosition.x;
        int currentY = currentTile.boardPosition.z; // z represents the world point, but it also represents the y point in the 2D array. 

        //Check each tile
        for (int i = 1; i <= movement; i++)
        {
            currentX += xDirection;
            currentY += yDirection;

            TileState tileState = currentTile.board.ValidateTile(currentX, currentY, this);

            if (tileState != TileState.Free) //if the tile is out of bounds or has a friendly on it, then break the look and don't add anything to the available target tiles.
                break;

            moveableTiles.Add(currentTile.board.allTiles[currentX, currentY]); //add the tile if it is free

        }
    }

    public virtual void CheckPath()
    {
        moveableTiles.Clear();

        //horizantal 
        CreateTilePath(1, 0, movement.x);
        CreateTilePath(-1, 0, movement.x);

        //horizantal 
        CreateTilePath(0, 1, movement.z);
        CreateTilePath(0, -1, movement.z);

        //horizantal 
        CreateTilePath(1, 1, movement.y);
        CreateTilePath(-1, 1, movement.y);

        //horizantal 
        CreateTilePath(-1, -1, movement.y);
        CreateTilePath(1, -1, movement.y);
    }

    public void ShowHighlightedTiles()
    {
        foreach (Tile tile in moveableTiles)
            tile.Focus();

            
    }

    public void HideHighlightedTiles()
    {
        foreach (Tile tile in moveableTiles)
            tile.RemoveFocus();


        moveableTiles.Clear();
    }
    public void Place(Tile newTile)
    {
        //tile
        currentTile = newTile;

        currentTile.currentUnit = this;

        //object
        transform.position = newTile.transform.position;
        gameObject.SetActive(true);
        GameManager.STATIC_SetUnitSelected(false);
    }


    public virtual void Move(Tile targetTile)
    {
        //Set previous tile
        previousTile = currentTile;

        //clear the current tile
        currentTile.currentUnit = null;

        //change current tile
        currentTile = targetTile;
        currentTile.currentUnit = this;

        //Move on board
        transform.position = currentTile.transform.position;

        if (!GameManager.aiEvaluationInProgress)
        {
            TransitionToState(idleState);
            GameManager.Static_SwitchSides(teamColor, characterID, targetTile.tileID);
            _audioSource.Play();
        }

        targetTile = null;


    }

    #region Mouse events
    public void OnMouseOver()
    {
        if (!GameManager.aiEvaluationInProgress && !GameManager.unitSelected && !UIManager.gamePaused)
        {
            if (gameObject.CompareTag("Interactive"))
            {
                // Test for cells
                CheckPath();
                ShowHighlightedTiles(); //show highlighted tiles
            }
        }
    }

    private void OnMouseExit()
    {
        if (!GameManager.aiEvaluationInProgress)
        {
            if (_currentState != hoverState) //don't clear the highlighted tiles while the player is draging a unit
            {
                HideHighlightedTiles();
            }
        }
       
        
    }

    public void Clicked()
    {
        TransitionToState(hoverState);
        GameManager.STATIC_SetUnitSelected(true);

    }

    public void Drag()
    {
        Ray ray = UXManager.mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit)) // did the ray hit anything?
        {
            transform.position = new Vector3(hit.point.x, transform.position.y, hit.point.z);// follow the ray(the cursor)

            foreach (Tile tile in moveableTiles)
            {
                if (hit.transform.gameObject.GetComponent<Tile>() == tile) // is the cursor over one of he highlighted tiles?
                {
                    if (Input.GetMouseButtonDown(0))  // right button clicked
                    {
                        Move(tile);//move to the tile the player has chosen 
                        GameManager.STATIC_SetUnitSelected(false);
                        _audioSource.Play();
                        break;// only one can be the target 
                    }
                    
                }

            }


            if (Input.GetMouseButtonDown(1)) // right button clicked
            {
                GameManager.STATIC_SetUnitSelected(false);

                TransitionToState(idleState);
                

            }
        }
    }

    #endregion

    #endregion

    #region States machine

    UnitBaseState _currentState;
    public UnitBaseState currentState { get { return _currentState; } }


    public readonly UnitAttackState attackState = new UnitAttackState(); // attack enemies
    public readonly UnitHoverState hoverState = new UnitHoverState();  // hover over enemies
    public readonly UnitIdleState idleState = new UnitIdleState(); // scan for enemies

    void Start()
    {
        TransitionToState(idleState);
    }
    private void Update()
    {
        if(gameObject!=null) //just in case it was destroyed between frames
         currentState.Update(this);
    }
    public void TransitionToState(UnitBaseState state)
    {
        _currentState = state;
        _currentState.EnterState(this);
    }


    #endregion

    #region Attack
    public BaseUnit target;
    public List<BaseUnit> targets;
    public Vector3 targetPos;
    public float coolDown;
    public int attackLimit, attackCount;

    Renderer _rend;
    public virtual void IdleUpdate() { CheckForEnemy(); }
    public virtual BaseUnit CheckForEnemy() { return null; }
    public virtual List<BaseUnit> CheckForEnemies(bool checkForReturn) { return null; }

    public virtual void Attack() 
    {

        bool canAttack = ValidateAttack();

        if (canAttack)
        {
            if (brain != null)
            {
                brain.IncreaseTileWeightSimple(characterID[1],target.characterID[1]);// this was a good placement of the unit because it can attack another. It's not going to loose its brain because the attack must be proven to be successful/unsucessful
            }
            if (target != null)
            {
                char characterCode = characterID[1];
                if (characterCode == 'R' || characterCode == 'M')
                    transform.LookAt(target.transform);
                StartCoroutine(target.TakeDamage(damage, characterID[1]));             //attack 

            }
        }
        else
        {
            if (brain != null)
            {
                brain.IncreaseUnitWeight(characterID[1]); // increase weight of unit if another unit has been killed or moved out of way 
                brain = null; //i dont want it to increase after every single kill
            }
            TransitionToState(idleState); //go back to idle
        }
    }
    public virtual bool ValidateAttack() {

        if (attackCount < attackLimit)
        {
            attackCount++;

            if (!target.gameObject.activeSelf) //if gameObject has been set to deactive
                return false;


            if (target.transform.position == targetPos)
                return true;

        }
        else
        {
            return false;
        }    

        return false;
    }

   

    #endregion
    public virtual void Die() {
        if (target.isActiveAndEnabled)
        {
            target.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        }


        gameObject.SetActive(false);
    }

    public IEnumerator TakeDamage(int damage, char attacker)
    {
        if (gameObject != null) // just already dead
        {
            _health -= damage;
             _rend.material.EnableKeyword("_EMISSION");
                if (_health <= 0)
                {
                    GameManager.Static_UnitDeath(teamColor);
                    Die();

                }
        }

        yield return new WaitForSeconds(.5f);

        if (gameObject != null) // just already dead
        {
            gameObject.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");

            if (brain != null)
            {
                brain.DecreaseTileWeights(characterID[1], attacker);
            }
        }

    }

    #region MachineLearning
    private void OnDestroy()
    {
        if (brain != null)
            brain.DecreaseUnitWeight(characterID[1]); // if i die, its a bad
    }

    public void SetBrain(Brain Brain)
    {
        brain = Brain;
    }
    #endregion


}
