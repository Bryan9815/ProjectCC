using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Credits : MonoBehaviour
{
    MainMenuController mainMenuController;
    TextMeshProUGUI exitText;
    bool firstStart = true;

    // Use this for initialization
    void Start()
    {
        mainMenuController = transform.parent.GetComponent<MainMenuController>();
        exitText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        exitText.text = "Press " + GameData.instance.interact.ToString() + " to return to menu";
        firstStart = false;
    }

    private void OnEnable()
    {
        if (!firstStart)
            exitText.text = "Press " + GameData.instance.interact.ToString() + " to return to menu";
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(GameData.instance.interact))
        {
            mainMenuController.TogglePanel(false, MainMenuController.MainMenuPanels.Credits);
        }
    }
}
