using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOptions : MonoBehaviour
{
    MainMenuController mainMenuController;
    Transform selector;
    float volume;
    int selectNum, maxSelectNum, resolutionNum, maxResolutionNum, windowMode;
    TextMeshProUGUI volumeText, resText, windowText;
    // Use this for initialization
    void Start ()
    {
        mainMenuController = transform.parent.parent.GetComponent<MainMenuController>();
        selector = transform.Find("Selector");

        selectNum = 0;
        maxSelectNum = 6;

        volume = BayatGames.SaveGameFree.SaveGame.Load<float>("Volume", 1);
        resolutionNum = BayatGames.SaveGameFree.SaveGame.Load("resolution", 5);
        maxResolutionNum = 5;
        windowMode = BayatGames.SaveGameFree.SaveGame.Load("windowMode", 0);

        volumeText = transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
        resText = transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>();
        windowText = transform.GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>();

        selector.localPosition = new Vector3(selector.localPosition.x, transform.GetChild(selectNum).localPosition.y, 0);
        UpdateWindowMode();
        UpdateVolumeText();
    }

    private void OnEnable()
    {
        Start();
    }

    // Update is called once per frame
    void Update()
    {
        #region Selection up/down
        if (Input.GetKeyDown(GameData.instance.playerKeys.down))
        {
            if (selectNum <= maxSelectNum)
            {
                if (selectNum == maxSelectNum)
                    selectNum = 0;
                else
                    selectNum++;
                selector.localPosition = new Vector3(selector.localPosition.x, transform.GetChild(selectNum).localPosition.y, 0);
            }
        }
        if (Input.GetKeyDown(GameData.instance.playerKeys.up))
        {
            if (selectNum >= 0)
            {
                if (selectNum == 0)
                    selectNum = maxSelectNum;
                else
                    selectNum--;

                selector.localPosition = new Vector3(selector.localPosition.x, transform.GetChild(selectNum).localPosition.y, selector.localPosition.z);
            }
        }
        #endregion

        #region Interact Button
        if (Input.GetKeyDown(GameData.instance.interact))
        {
            switch (selectNum)
            {
                case 3: // Keybinding
                    mainMenuController.ToggleKeyBindingPanel(true);
                    break;
                case 4: // Reset Game Options to default
                    ResetGameSettings();
                    break;
                case 5: // Erase Game Data
                    BayatGames.SaveGameFree.SaveGame.Clear();
                    GameData.instance.LoadAllData();
                    ResetGameSettings();
                    transform.GetChild(selectNum).GetComponent<TextMeshProUGUI>().text = "Data reset!";
                    transform.GetChild(selectNum).GetComponent<TextMeshProUGUI>().color = Color.red;
                    break;
                case 6: // Exit
                    BayatGames.SaveGameFree.SaveGame.Save<float>("Volume", volume);
                    BayatGames.SaveGameFree.SaveGame.Save("resolution", resolutionNum);
                    BayatGames.SaveGameFree.SaveGame.Save("windowMode", windowMode);
                    mainMenuController.TogglePanel(false, MainMenuController.MainMenuPanels.Options);
                    break;
            }
        }
        #endregion

        #region Game Options Left/Right
        if (Input.GetKeyDown(GameData.instance.playerKeys.left))
        {
            switch (selectNum)
            {
                case 1:
                    if (resolutionNum >= 0)
                    {
                        if (resolutionNum == 0)
                            resolutionNum = maxResolutionNum;
                        else
                            resolutionNum--;
                        UpdateResolution();
                    }
                    break;
                case 2:
                    if (windowMode >= 0)
                    {
                        if (windowMode == 0)
                            windowMode = 1;
                        else
                            windowMode--;
                        UpdateWindowMode();
                    }
                    break;
            }
        }
        if (Input.GetKeyDown(GameData.instance.playerKeys.right))
        {
            switch (selectNum)
            {
                case 1:
                    if (resolutionNum <= maxResolutionNum)
                    {
                        if (resolutionNum == maxResolutionNum)
                            resolutionNum = 0;
                        else
                            resolutionNum++;
                        UpdateResolution();
                    }
                    break;
                case 2:
                    if (windowMode <= 1)
                    {
                        if (windowMode == 1)
                            windowMode = 0;
                        else
                            windowMode++;
                        UpdateWindowMode();
                    }
                    break;
            }
        }
        if (Input.GetKey(GameData.instance.playerKeys.left))
        {
            Debug.Log("volume: " + volume);
            if (selectNum == 0)
            {
                if (volume > 0)
                    volume -= 0.1f * Time.deltaTime;
                UpdateVolumeText();
            }
        }
        if (Input.GetKey(GameData.instance.playerKeys.right))
        {
            if (selectNum == 0)
            {
                if (volume < 1)
                    volume += 0.1f * Time.deltaTime;
                UpdateVolumeText();
            }
        }
        #endregion
    }

    void ResetGameSettings()
    {
        volume = 1;
        resolutionNum = 5;
        windowMode = 0;
        UpdateVolumeText();
        GameData.instance.ResetKeySettings();
        UpdateWindowMode();
    }

    void UpdateVolumeText()
    {
        volumeText.text = ((int)(volume * 100)).ToString();
    }

    void UpdateResolution()
    {
        switch(resolutionNum)
        {
            case 0:
                resText.text = "800x600";
                if (windowMode == 0)
                    Screen.SetResolution(800, 600, true);
                else
                    Screen.SetResolution(800, 600, false);
                break;
            case 1:
                resText.text = "1024x720";
                if (windowMode == 0)
                    Screen.SetResolution(1024, 720, true);
                else
                    Screen.SetResolution(1024, 720, false);
                break;
            case 2:
                resText.text = "1440x810";
                if (windowMode == 0)
                    Screen.SetResolution(1440, 810, true);
                else
                    Screen.SetResolution(1440, 810, false);
                break;
            case 3:
                resText.text = "1600x900";
                if (windowMode == 0)
                    Screen.SetResolution(1600, 900, true);
                else
                    Screen.SetResolution(1600, 900, false);
                break;
            case 4:
                resText.text = "1600x1024";
                if (windowMode == 0)
                    Screen.SetResolution(1600, 1024, true);
                else
                    Screen.SetResolution(1600, 1024, false);
                break;
            case 5:
                resText.text = "1920x1080";
                if (windowMode == 0)
                    Screen.SetResolution(1920, 1080, true);
                else
                    Screen.SetResolution(1920, 1080, false);
                break;
        }
    }

    void UpdateWindowMode()
    {
        if (windowMode == 0)
            windowText.text = "Fullscreen";
        else if (windowMode == 1)
            windowText.text = "Windowed";

        UpdateResolution();
    }
}
