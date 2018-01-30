using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Highscore : MonoBehaviour
{
    MainMenuController mainMenuController;
    Transform stats;
    TextMeshProUGUI exitText;
    bool firstStart = true;

	// Use this for initialization
	void Start ()
    {
        mainMenuController = transform.parent.GetComponent<MainMenuController>();

        stats = transform.GetChild(0).GetChild(1);

        exitText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        UpdateText();

        firstStart = false;
    }

    private void OnEnable()
    {
        if(!firstStart)
            UpdateText();
    }

    void UpdateText()
    {
        Debug.Log(exitText);
        exitText.text = "Press " + GameData.instance.interact.ToString() + " to return to menu";

        stats.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Most Kills in a single run: " + GameData.instance.killRecord;
        stats.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Total Kills: " + GameData.instance.totalKills;
        stats.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Total Rooms Cleared: " + GameData.instance.totalRooms;
        stats.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Total Power Ups Obtained: " + GameData.instance.totalPowerUps;
        stats.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Total money picked up: " + GameData.instance.totalMoney;
        stats.GetChild(5).GetComponent<TextMeshProUGUI>().text = "Total runs completed: " + GameData.instance.totalRunsCompleted;
        stats.GetChild(6).GetComponent<TextMeshProUGUI>().text = "Total runs failed: " + GameData.instance.totalRunsFailed;
        stats.GetChild(7).GetComponent<TextMeshProUGUI>().text = "Total runs quit: " + GameData.instance.totalRunsQuit;
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(GameData.instance.interact))
        {
            mainMenuController.TogglePanel(false, MainMenuController.MainMenuPanels.Highscore);
        }
	}
}
