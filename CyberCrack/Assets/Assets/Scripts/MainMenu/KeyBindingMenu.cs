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

    private void OnEnable()
    {
        selectNum = 0;
        UpdateSelectorPosition();
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
        for(int i = 0; i < GameData.instance.keyList.Count; i++)
        {
            keys.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = GameData.instance.keyList[i].ToString();
            keys.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().color = Color.black;
        }
    }

    void UpdateKeyBindings(KeyCode key)
    {
        GameData.instance.CheckAndUpdateKey(selectNum, key);
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
