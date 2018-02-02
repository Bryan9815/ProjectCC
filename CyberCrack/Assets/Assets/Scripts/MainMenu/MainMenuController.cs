using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    GameObject MenuOptions, OptionsPanel, KeyBindingPanel, HighscorePanel, CreditsPanel;
    AudioSource sound;

    public enum MainMenuPanels { Options, Highscore, Credits };

    public bool panelOpen;
	// Use this for initialization
	void Start ()
    {
        sound = GetComponent<AudioSource>();
        sound.volume = GameData.instance.GetVolume();

        MenuOptions = transform.GetChild(1).gameObject;
        OptionsPanel = transform.GetChild(2).gameObject;
        KeyBindingPanel = OptionsPanel.transform.GetChild(1).gameObject;
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

    public void ToggleKeyBindingPanel(bool active)
    {
        KeyBindingPanel.SetActive(active);
        OptionsPanel.transform.GetChild(0).GetComponent<GameOptions>().enabled = !active;
    }
}
