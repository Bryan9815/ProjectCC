using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyBindingMenu : MonoBehaviour
{
    MainMenuController mainMenuController;
    Transform keys;
    Transform selector;
    int selectNum, maxSelectNum;
    bool changingKeyBinding;
    // Use this for initialization
    void Start ()
    {
        mainMenuController = transform.parent.parent.GetComponent<MainMenuController>();
        keys = transform.GetChild(0);
        selector = transform.Find("Selector");

        selectNum = 0;
        maxSelectNum = 11;

        changingKeyBinding = false;
        RefreshKeyBindingText();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!changingKeyBinding)
        {
            if (Input.GetKeyDown(GameData.instance.playerKeys.down))
            {
                if (selectNum <= maxSelectNum)
                {
                    if (selectNum == maxSelectNum)
                        selectNum = 0;
                    else
                        selectNum++;

                    UpdateSelectorPosition();
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

                    UpdateSelectorPosition();
                }
            }
            if (Input.GetKeyDown(GameData.instance.interact))
            {
                if (selectNum == maxSelectNum)
                    mainMenuController.ToggleKeyBindingPanel(false);
                else
                    changingKeyBinding = true;
            }
        }

        if(changingKeyBinding)
        {
            keys.GetChild(selectNum).GetChild(2).GetComponent<TextMeshProUGUI>().color = Color.white;

            StartCoroutine(getInput());
        }
    }

    IEnumerator getInput()
    {
        yield return new WaitForSeconds(0.1f);
        if (Input.anyKeyDown)
        {
            changingKeyBinding = false;
            UpdateKeyBindings(detectPressedKeyOrButton());
        }
    }

    void UpdateSelectorPosition()
    {
        if (selectNum != maxSelectNum)
            selector.position = new Vector3(selector.position.x, keys.GetChild(selectNum).position.y, 0);
        else // Exit
            selector.position = new Vector3(selector.position.x, transform.GetChild(1).position.y, 0);
    }

    void RefreshKeyBindingText()
    {
        for(int i = 0; i < maxSelectNum; i++)
        {
            switch(i)
            {
                case 0:
                    keys.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = GameData.instance.playerKeys.up.ToString();
                    break;
                case 1:
                    keys.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = GameData.instance.playerKeys.down.ToString();
                    break;
                case 2:
                    keys.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = GameData.instance.playerKeys.left.ToString();
                    break;
                case 3:
                    keys.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = GameData.instance.playerKeys.right.ToString();
                    break;
                case 4:
                    keys.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = GameData.instance.shootKeys.up.ToString();
                    break;
                case 5:
                    keys.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = GameData.instance.shootKeys.down.ToString();
                    break;
                case 6:
                    keys.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = GameData.instance.shootKeys.left.ToString();
                    break;
                case 7:
                    keys.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = GameData.instance.shootKeys.right.ToString();
                    break;
                case 8:
                    keys.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = GameData.instance.interact.ToString();
                    break;
                case 9:
                    keys.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = GameData.instance.pauseOpen.ToString();
                    break;
                case 10:
                    keys.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = GameData.instance.miniMap.ToString();
                    break;
            }
            keys.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().color = Color.black;
        }
    }

    void UpdateKeyBindings(KeyCode key)
    {
        //switch (selectNum)
        //{
        //    case 0:                
        //        BayatGames.SaveGameFree.SaveGame.Save("playerUp", key.ToString());
        //        GameData.instance.CheckOtherKeys(GameData.instance.playerKeys.up, key);
        //        break;
        //    case 1:
        //        BayatGames.SaveGameFree.SaveGame.Save("playerDown", key.ToString());
        //        GameData.instance.CheckOtherKeys(GameData.instance.playerKeys.down, key);
        //        break;
        //    case 2:
        //        BayatGames.SaveGameFree.SaveGame.Save("playerLeft", key.ToString());
        //        GameData.instance.CheckOtherKeys(GameData.instance.playerKeys.left, key);
        //        break;
        //    case 3:
        //        BayatGames.SaveGameFree.SaveGame.Save("playerRight", key.ToString());
        //        GameData.instance.CheckOtherKeys(GameData.instance.playerKeys.right, key);
        //        break;
        //    case 4:
        //        BayatGames.SaveGameFree.SaveGame.Save("shootUp", key.ToString());
        //        GameData.instance.CheckOtherKeys(GameData.instance.shootKeys.up, key);
        //        break;
        //    case 5:
        //        BayatGames.SaveGameFree.SaveGame.Save("shootDown", key.ToString());
        //        GameData.instance.CheckOtherKeys(GameData.instance.shootKeys.down, key);
        //        break;
        //    case 6:
        //        BayatGames.SaveGameFree.SaveGame.Save("shootLeft", key.ToString());
        //        GameData.instance.CheckOtherKeys(GameData.instance.shootKeys.left, key);
        //        break;
        //    case 7:
        //        BayatGames.SaveGameFree.SaveGame.Save("shootRight", key.ToString());
        //        GameData.instance.CheckOtherKeys(GameData.instance.shootKeys.right, key);
        //        break;
        //    case 8:
        //        BayatGames.SaveGameFree.SaveGame.Save("interact", key.ToString());
        //        GameData.instance.CheckOtherKeys(GameData.instance.interact, key);
        //        break;
        //    case 9:
        //        BayatGames.SaveGameFree.SaveGame.Save("pauseOpen", key.ToString());
        //        GameData.instance.CheckOtherKeys(GameData.instance.pauseOpen, key);
        //        break;
        //    case 10:
        //        BayatGames.SaveGameFree.SaveGame.Save("miniMap", key.ToString());
        //        GameData.instance.CheckOtherKeys(GameData.instance.miniMap, key);
        //        break;
        //}

        GameData.instance.CheckOtherKeys(selectNum, key);
        //GameData.instance.LoadKeySettings();
        RefreshKeyBindingText();
    }

    KeyCode detectPressedKeyOrButton()
    {
        KeyCode keyCode = KeyCode.A;

        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kcode))
                keyCode = kcode;
        }

        return keyCode;
    }
}
