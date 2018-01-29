﻿using System.Collections;
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
        mainMenuController = transform.parent.GetComponent<MainMenuController>();

        selectNum = 0;
        maxSelectNum = 6;

        volume = BayatGames.SaveGameFree.SaveGame.Load("Volume", 1);
        resolutionNum = BayatGames.SaveGameFree.SaveGame.Load("resolution", 5);
        maxResolutionNum = 5;
        windowMode = BayatGames.SaveGameFree.SaveGame.Load("windowMode", 0);

        volumeText = transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
        resText = transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>();
        windowText = transform.GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>();

        UpdateWindowMode();
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown(GameData.instance.interact))
        {
            switch(selectNum)
            {
                case 3:
                    mainMenuController.ToggleKeyBindingPanel(true);
                    break;
                case 4:
                    volume = 1;
                    resolutionNum = 5;
                    windowMode = 0;
                    UpdateVolumeText();
                    GameData.instance.ResetKeySettings();
                    UpdateWindowMode();
                    break;
                case 5:
                    BayatGames.SaveGameFree.SaveGame.Clear();
                    transform.GetChild(selectNum).GetComponent<TextMeshProUGUI>().text = "Data reset!";
                    transform.GetChild(selectNum).GetComponent<TextMeshProUGUI>().color = Color.red;
                    break;
                case 6:
                    BayatGames.SaveGameFree.SaveGame.Save("Volume", volume);
                    BayatGames.SaveGameFree.SaveGame.Save("resolution", resolutionNum);
                    BayatGames.SaveGameFree.SaveGame.Save("windowMode", windowMode);
                    mainMenuController.TogglePanel(false, MainMenuController.MainMenuPanels.Options);
                    break;
            }
        }
        if (Input.GetKeyDown(GameData.instance.playerKeys.left))
        {
            switch (selectNum)
            {
                case 0:
                    if(volume > 0)
                        volume -= 0.01f;
                    UpdateVolumeText();
                    break;
                case 1:
                    if (windowMode > 0)
                    {
                        windowMode--;
                        UpdateWindowMode();
                    }
                    break;
                case 2:
                    if (resolutionNum > 0)
                    {
                        resolutionNum--;
                        UpdateResolution();
                    }
                    break;
            }
        }
        if (Input.GetKeyDown(GameData.instance.playerKeys.right))
        {
            switch (selectNum)
            {
                case 0:
                    if(volume < 1)
                        volume += 0.01f;
                    UpdateVolumeText();
                    break;
                case 1:
                    if (windowMode > 0)
                    {
                        windowMode--;
                        UpdateWindowMode();
                    }
                    break;
                case 2:
                    if (resolutionNum > 0)
                    {
                        resolutionNum--;
                        UpdateResolution();
                    }
                    break;
            }
        }
    }

    void UpdateVolumeText()
    {
        volumeText.text = (volume * 100).ToString();
    }

    void UpdateResolution()
    {
        switch(resolutionNum)
        {
            case 0:
                resText.text = "800 x 600";
                if (windowMode == 0)
                    Screen.SetResolution(800, 600, true);
                else
                    Screen.SetResolution(800, 600, false);
                break;
            case 1:
                resText.text = "1024 x 720";
                if (windowMode == 0)
                    Screen.SetResolution(1024, 720, true);
                else
                    Screen.SetResolution(1024, 720, false);
                break;
            case 2:
                resText.text = "  1440 x 810";
                if (windowMode == 0)
                    Screen.SetResolution(1440, 810, true);
                else
                    Screen.SetResolution(1440, 810, false);
                break;
            case 3:
                resText.text = "  1600 x 900";
                if (windowMode == 0)
                    Screen.SetResolution(1600, 900, true);
                else
                    Screen.SetResolution(1600, 900, false);
                break;
            case 4:
                resText.text = " 1600 x 1024";
                if (windowMode == 0)
                    Screen.SetResolution(1600, 1024, true);
                else
                    Screen.SetResolution(1600, 1024, false);
                break;
            case 5:
                resText.text = " 1920 x 1080";
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