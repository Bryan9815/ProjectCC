using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    GameObject MenuOptions, OptionsPanel, HighscorePanel, CreditsPanel;

    public enum MainMenuPanels { Options, Highscore, Credits };

    public bool panelOpen;
	// Use this for initialization
	void Start ()
    {
        MenuOptions = transform.GetChild(1).gameObject;
        OptionsPanel = transform.GetChild(2).gameObject;
        HighscorePanel = transform.GetChild(3).gameObject;
        CreditsPanel = transform.GetChild(4).gameObject;

        panelOpen = false;
    }

    private void Update()
    {
        
    }

    public void TogglePanel(bool active, MainMenuPanels panels)
    {
        panelOpen = active;
        switch(panels)
        {
            case MainMenuPanels.Options:
                OptionsPanel.SetActive(active);
                MenuOptions.GetComponent<MenuOptions>().enabled = !active;
                break;
            case MainMenuPanels.Highscore:
                HighscorePanel.SetActive(active);
                MenuOptions.GetComponent<MenuOptions>().enabled = !active;
                break;
            case MainMenuPanels.Credits:
                CreditsPanel.SetActive(active);
                MenuOptions.GetComponent<MenuOptions>().enabled = !active;
                break;
        }
    }
}
