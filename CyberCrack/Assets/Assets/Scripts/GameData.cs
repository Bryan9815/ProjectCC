using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BayatGames.SaveGameFree;

public class GameData : MonoBehaviour
{
    public static GameData instance;
    public PlayerKeys playerKeys, shootKeys;
    [HideInInspector]
    public KeyCode interact, pauseOpen, miniMap;
    
    void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;//Avoid doing anything else
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        LoadKeySettings();
    }

    void LoadKeySettings()
    {
        playerKeys.up = (KeyCode)System.Enum.Parse(typeof(KeyCode), SaveGame.Load("playerUp", "W"));
        playerKeys.down = (KeyCode)System.Enum.Parse(typeof(KeyCode), SaveGame.Load("playerDown", "S"));
        playerKeys.left = (KeyCode)System.Enum.Parse(typeof(KeyCode), SaveGame.Load("playerLeft", "A"));
        playerKeys.right = (KeyCode)System.Enum.Parse(typeof(KeyCode), SaveGame.Load("playerRight", "D"));

        shootKeys.up = (KeyCode)System.Enum.Parse(typeof(KeyCode), SaveGame.Load("shootUp", "UpArrow"));
        shootKeys.down = (KeyCode)System.Enum.Parse(typeof(KeyCode), SaveGame.Load("shootDown", "DownArrow"));
        shootKeys.left = (KeyCode)System.Enum.Parse(typeof(KeyCode), SaveGame.Load("shootLeft", "LeftArrow"));
        shootKeys.right = (KeyCode)System.Enum.Parse(typeof(KeyCode), SaveGame.Load("shootRight", "RightArrow"));

        interact = (KeyCode)System.Enum.Parse(typeof(KeyCode), SaveGame.Load("interact", "Space"));
        pauseOpen = (KeyCode)System.Enum.Parse(typeof(KeyCode), SaveGame.Load("pauseOpen", "Escape"));
        miniMap = (KeyCode)System.Enum.Parse(typeof(KeyCode), SaveGame.Load("miniMap", "M"));
    }
}

public struct PlayerKeys
{
    public KeyCode up, down, left, right;
}