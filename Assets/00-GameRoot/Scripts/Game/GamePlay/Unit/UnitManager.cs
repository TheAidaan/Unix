using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{

    GameObject _unitPrefab;

    MiniMax _minMax = null;
    Brain _brain = null;

    char[] _unitOrder = new char[12]
    {
                'R','M','W','M','R','M',
                'M','R','W','R','M','M'
    };

    Dictionary<char, Type> _unitLibrary = new Dictionary<char, Type>()
    {
        {'R', typeof(RangedUnit) },
        {'M', typeof(MeleeUnit) },
        {'W', typeof(WizardUnit) },

    };


    private void Awake()
    {
        _unitPrefab = Resources.Load<GameObject>("Prefabs/BaseUnit");
    }

    #region Unit setup

    public void Setup(Board board)
    {
        GameManager.STATIC_SetUnitsGameData( CreateUnits(Color.red, new Color32(210, 95, 64, 255), board), Color.red);

        GameManager.STATIC_SetUnitsGameData(CreateUnits(Color.blue, new Color32(80, 124, 159, 255),board), Color.blue);


        PlaceUnits(1, 0, GameManager.gameData.GetRedUnits(), board);
        PlaceUnits(GameManager.gameData.GetBoardLength() - 2, GameManager.gameData.GetBoardLength() - 1, GameManager.gameData.GetBlueUnits(), board);


        
        if (GameManager.gameData.SpectatorBattle())
        {
            int rand = UnityEngine.Random.Range(0, 2);

            _minMax = GetComponent<MiniMax>();
            _brain = GetComponent<Brain>();


            if (rand == 1)
            {
                _brain.AssignUnits();
                _minMax.AssignUnits();
            }
            else
            {
                _minMax.AssignUnits();
                _brain.AssignUnits();
            }


            SetInteractive(GameManager.gameData.GetRedUnits(), false);
            SetInteractive(GameManager.gameData.GetBlueUnits(), false);
            SwitchSides(Color.red);
        }
        else
        {
            if (GameManager.gameData.LoadMinMaxScript())
            {
                _minMax = GetComponent<MiniMax>();
                _minMax.AssignUnits();
            }

            if (GameManager.gameData.LoadMachineLearningScript())
            {
                _brain = GetComponent<Brain>();
                _brain.AssignUnits();

            }

            SwitchSides(Color.red);
        }
    }

    List<BaseUnit> CreateUnits(Color teamColor, Color32 unitColor,Board board)
    {
        List<BaseUnit> newUnits = new List<BaseUnit>();

        for(int i = 0; i< _unitOrder.Length; i++)
        {
            Vector3 rotation = teamColor == Color.red ? new Vector3(0, 0, 0) : new Vector3(0, 180, 0);

            // instantiate the new unit
            GameObject newUnitObject = Instantiate(_unitPrefab, transform);
            newUnitObject.transform.eulerAngles = rotation;

            //identify the new unit
            char key = _unitOrder[i];
            Type unitType = _unitLibrary[key];

            //store the new unit
            BaseUnit newUnit = (BaseUnit)newUnitObject.AddComponent(unitType);
            newUnits.Add(newUnit);

            //setup peice
            char color = teamColor == Color.red ? 'R' : 'B';
            string id = color.ToString() + key + i.ToString();
            newUnit.Setup(teamColor, unitColor, id);
        }

        return newUnits;
    }

    void PlaceUnits(int secondRow,int firstRow,List<BaseUnit> units, Board board)
    {
        int offset = GameManager.gameData.ComplexBoard() ? 2 : 1;

        for (int i = 0; i < 6; i++)
        {
            units[i].Place(board.allTiles[i+ offset, secondRow]);

            units[i+6].Place(board.allTiles[i+ offset, firstRow]);
        }
    }

    #endregion

    #region Unit controls

    void SetInteractive(List<BaseUnit> allUnits, bool value)
    {
        string tag = value ? "Interactive" : "Untagged";

        foreach (BaseUnit unit in allUnits)
            if (unit.gameObject.activeSelf)
                unit.gameObject.tag = tag;         
    }

    public void SwitchSides(Color colortThatJustPlayed)
    {

        if (!GameManager.gameData.SpectatorBattle())
        {
            bool isRedTurn = colortThatJustPlayed == Color.red ? true : false;

            //set the interactivity
            SetInteractive(GameManager.gameData.GetRedUnits(), !isRedTurn);
            SetInteractive(GameManager.gameData.GetBlueUnits(), isRedTurn);

            if (GameManager.gameData.GetPlayerColor() == colortThatJustPlayed) // the player just went and it is the ai's turn  {
            {
                if (_minMax != null)
                {
                    if (GameManager.gameData.GetMinMaxColor() == Color.red)
                        SetInteractive(GameManager.gameData.GetRedUnits(), false);
                    else
                        SetInteractive(GameManager.gameData.GetBlueUnits(), false);


                    GameManager.play += _minMax.Play;
                }

                if (_brain != null)
                {
                    if (GameManager.gameData.GetGeneticAIColor() == Color.red)
                        SetInteractive(GameManager.gameData.GetRedUnits(), false);
                    else
                        SetInteractive(GameManager.gameData.GetBlueUnits(), false);

                    GameManager.play += _brain.Play;
                }

            }
        }
        else
        {
            if (_brain != null)
            {
                if (GameManager.gameData.GetGeneticAIColor() != colortThatJustPlayed)
                    GameManager.play += _brain.Play;
            }

            if (_minMax != null)
            {
                if (GameManager.gameData.GetMinMaxColor() != colortThatJustPlayed)
                    GameManager.play += _minMax.Play;
            }
        }

        GameManager.Static_Play();
           
                
    }



    #endregion

    
}
