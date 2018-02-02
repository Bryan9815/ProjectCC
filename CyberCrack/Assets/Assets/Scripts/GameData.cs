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
    [HideInInspector]
    public List<KeyCode> keyList = new List<KeyCode>();
    [HideInInspector]
    public int killRecord, totalKills, totalRooms, totalPowerUps, totalMoney, totalRunsCompleted, totalRunsFailed, totalRunsQuit;
    
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

        LoadAllData();
        RefreshKeyList();
    }

    public float GetVolume()
    {
        return SaveGame.Load<float>("Volume", 1);
    }

    public void LoadAllData()
    {
        LoadHighScoreStats();
        LoadKeySettings();
    }

    public void LoadHighScoreStats()
    {
        killRecord = SaveGame.Load("killRecord", 0);
        totalKills = SaveGame.Load("totalKills", 0);
        totalRooms = SaveGame.Load("totalRooms", 0);
        totalPowerUps = SaveGame.Load("totalPowerUps", 0);
        totalMoney = SaveGame.Load("totalMoney", 0);
        totalRunsCompleted = SaveGame.Load("totalRunsCompleted", 0);
        totalRunsFailed = SaveGame.Load("totalRunsFailed", 0);
        totalRunsQuit = SaveGame.Load("totalRunsQuit", 0);
    }

    public void UpdateData(string name, int data)
    {
        SaveGame.Save(name, data);
        LoadHighScoreStats();
    }

    void RefreshKeyList()
    {
        if (keyList.Count > 0)
            keyList.Clear();

        keyList.Add(playerKeys.up);
        keyList.Add(playerKeys.down);
        keyList.Add(playerKeys.left);
        keyList.Add(playerKeys.right);
        keyList.Add(shootKeys.up);
        keyList.Add(shootKeys.down);
        keyList.Add(shootKeys.left);
        keyList.Add(shootKeys.right);
        keyList.Add(interact);
        keyList.Add(pauseOpen);
        keyList.Add(miniMap);
    }

    public void LoadKeySettings()
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

    public void ResetKeySettings()
    {
        SaveGame.Save("playerUp", KeyCode.W.ToString());
        SaveGame.Save("playerDown", KeyCode.S.ToString());
        SaveGame.Save("playerLeft", KeyCode.A.ToString());
        SaveGame.Save("playerRight", KeyCode.D.ToString());

        SaveGame.Save("shootUp", KeyCode.UpArrow.ToString());
        SaveGame.Save("shootDown", KeyCode.DownArrow.ToString());
        SaveGame.Save("shootLeft", KeyCode.LeftArrow.ToString());
        SaveGame.Save("shootRight", KeyCode.RightArrow.ToString());

        SaveGame.Save("interact", KeyCode.Space.ToString());
        SaveGame.Save("pauseOpen", KeyCode.Escape.ToString());
        SaveGame.Save("miniMap", KeyCode.M.ToString());

        LoadKeySettings();
        RefreshKeyList();
    }

    public void CheckAndUpdateKey(int index, KeyCode newKeyCode)
    {
        for(int i = 0; i < keyList.Count-1; i++)
        {
            if(keyList[i] != keyList[index])
            {
                if (keyList[i] == newKeyCode)
                {
                    UpdateKey(i, keyList[index]);
                }
            }
        }

        UpdateKey(index, newKeyCode);
        RefreshKeyList();
    }

    void UpdateKey(int index, KeyCode newKeyCode)
    {
        switch (index)
        {
            case 0:
                playerKeys.up = newKeyCode;
                SaveGame.Save("playerUp", newKeyCode.ToString());
                break;
            case 1:
                playerKeys.down = newKeyCode;
                SaveGame.Save("playerDown", newKeyCode.ToString());
                break;
            case 2:
                playerKeys.left = newKeyCode;
                SaveGame.Save("playerLeft", newKeyCode.ToString());
                break;
            case 3:
                playerKeys.right = newKeyCode;
                SaveGame.Save("playerRight", newKeyCode.ToString());
                break;
            case 4:
                shootKeys.up = newKeyCode;
                SaveGame.Save("shootUp", newKeyCode.ToString());
                break;
            case 5:
                shootKeys.down = newKeyCode;
                SaveGame.Save("shootDown", newKeyCode.ToString());
                break;
            case 6:
                shootKeys.left = newKeyCode;
                SaveGame.Save("shootLeft", newKeyCode.ToString());
                break;
            case 7:
                shootKeys.right = newKeyCode;
                SaveGame.Save("shootRight", newKeyCode.ToString());
                break;
            case 8:
                interact = newKeyCode;
                SaveGame.Save("interact", newKeyCode.ToString());
                break;
            case 9:
                pauseOpen = newKeyCode;
                SaveGame.Save("pauseOpen", newKeyCode.ToString());
                break;
            case 10:
                miniMap = newKeyCode;
                SaveGame.Save("miniMap", newKeyCode.ToString());
                break;
        }
    }

    public void UpdateWindow(int resolutionNum, int windowMode)
    {
        switch (resolutionNum)
        {
            case 0:
                if (windowMode == 0)
                    Screen.SetResolution(800, 600, true);
                else
                    Screen.SetResolution(800, 600, false);
                break;
            case 1:
                if (windowMode == 0)
                    Screen.SetResolution(1024, 720, true);
                else
                    Screen.SetResolution(1024, 720, false);
                break;
            case 2:
                if (windowMode == 0)
                    Screen.SetResolution(1440, 810, true);
                else
                    Screen.SetResolution(1440, 810, false);
                break;
            case 3:
                if (windowMode == 0)
                    Screen.SetResolution(1600, 900, true);
                else
                    Screen.SetResolution(1600, 900, false);
                break;
            case 4:
                if (windowMode == 0)
                    Screen.SetResolution(1600, 1024, true);
                else
                    Screen.SetResolution(1600, 1024, false);
                break;
            case 5:
                if (windowMode == 0)
                    Screen.SetResolution(1920, 1080, true);
                else
                    Screen.SetResolution(1920, 1080, false);
                break;
        }
    }
}

public struct PlayerKeys
{
    public KeyCode up, down, left, right;
}