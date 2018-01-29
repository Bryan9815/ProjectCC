using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOptions : MonoBehaviour
{
    MainMenuController mainMenuController;
    Transform selector;
    int selectNum, maxSelectNum;
	// Use this for initialization
	void Start ()
    {
        mainMenuController = transform.parent.GetComponent<MainMenuController>();
        selector = transform.GetChild(1);
        selectNum = 0;
        maxSelectNum = 4;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKeyDown(GameData.instance.playerKeys.down) && selectNum < maxSelectNum)
        {
            selectNum++;
            selector.localPosition = new Vector3(0, 200 - (100 * selectNum), 0);
        }
        if (Input.GetKeyDown(GameData.instance.playerKeys.up) && selectNum > 0)
        {
            selectNum--;
            selector.localPosition = new Vector3(0, 200 - (100 * selectNum), 0);
        }

        if(Input.GetKeyDown(GameData.instance.interact))
        {
            switch(selectNum)
            {
                case 0:
                    HelperFunctions.SceneTransition("Game");
                    break;
                case 1:
                    mainMenuController.TogglePanel(true, MainMenuController.MainMenuPanels.Options);
                    break;
                case 2:
                    mainMenuController.TogglePanel(true, MainMenuController.MainMenuPanels.Highscore);
                    break;
                case 3:
                    mainMenuController.TogglePanel(true, MainMenuController.MainMenuPanels.Credits);
                    break;
                case 4:
                    Application.Quit();
                    break;
            }
        }
    }
}
