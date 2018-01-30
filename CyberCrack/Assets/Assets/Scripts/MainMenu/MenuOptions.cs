using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuOptions : MonoBehaviour
{
    MainMenuController mainMenuController;
    Transform selector;
    int selectNum, maxSelectNum;
    bool selectBarFading;
    float selectBarFadeTimer;
    // Use this for initialization
    void Start ()
    {
        mainMenuController = transform.parent.GetComponent<MainMenuController>();
        selector = transform.GetChild(1);
        selectNum = 0;
        maxSelectNum = 4;

        selectBarFading = true;
        selectBarFadeTimer = 0;
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

        #region manual select bar blinking animation
        selectBarFadeTimer += Time.unscaledDeltaTime;
        if (selectBarFadeTimer >= 0.5f)
        {
            selectBarFadeTimer = 0;
            if (selectBarFading)
                selectBarFading = false;
            else
                selectBarFading = true;
        }

        if (selectBarFading)
        {
            Color newColor = new Color(selector.GetComponent<Image>().color.r, selector.GetComponent<Image>().color.g, selector.GetComponent<Image>().color.b, selector.GetComponent<Image>().color.a - (0.6f / 30));
            if (newColor.a < 0)
                newColor.a = 0;
            selector.GetComponent<Image>().color = newColor;
        }
        else
        {
            Color newColor = new Color(selector.GetComponent<Image>().color.r, selector.GetComponent<Image>().color.g, selector.GetComponent<Image>().color.b, selector.GetComponent<Image>().color.a + (0.6f / 30));
            if (newColor.a > 0.6f)
                newColor.a = 0.6f;
            selector.GetComponent<Image>().color = newColor;
        }
        #endregion
    }
}
