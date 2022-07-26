using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Game_UIManager : UIManager
{
    bool _showPauseScreen, _showSettings, _showGameOverScreen;

    [SerializeField]
    TextMeshProUGUI _txtInformation, _txtRedTeamScore, _txtBlueTeamScore, _txtWinner;

    private void Start()
    {


        if (!GameData.aiBattle)
        {
            Color vertexColor;
            string playerColor;
            if (GameData.geneticAIColor == Color.red || GameData.minMaxColor == Color.red)
            {
                vertexColor = new Color32(80, 124, 159, 255);
                playerColor = "blue";

            }
            else
            {
                vertexColor = new Color32(210, 95, 64, 255);
                playerColor = "red";

            }

            
            StartCoroutine(IEnumerator_DisplayInformation("you are playing as the " + playerColor + " team", vertexColor));
        }

    }



    public override void SetUI()
    { /*
        _childPanels[0] : Pause menu
        _childPanels[1] : Settings menu
        _childPanels[2] : Game Over Screen

      */
        _childPanels[0].SetActive(_showPauseScreen);
        _childPanels[1].SetActive(_showSettings);
        _childPanels[2].SetActive(_showGameOverScreen);

    }
   

   
   

    public void ShowSettingsMenu()
    {
        _showPauseScreen = false;
        _showSettings = true;
        SetUI();
    }

    IEnumerator IEnumerator_DisplayInformation(string information, Color vertexColor)
    {
        _txtInformation.gameObject.SetActive(true);
        _txtInformation.text = information;
        _txtInformation.color = vertexColor;


        yield return new WaitForSeconds(2.5f);

        _txtInformation.gameObject.SetActive(true);
        _txtInformation.text = string.Empty;


       
    }

}
