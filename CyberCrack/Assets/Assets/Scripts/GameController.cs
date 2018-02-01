using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public Transform gameplayCanvas;
    public Transform uiCanvas;
    [HideInInspector]
    public Transform bossHPBar;
    [HideInInspector]
    public GameObject pausePanel, deathPanel, victoryPanel;
    [HideInInspector]
    public RoomInstance currentRoom;
    [HideInInspector]
    public int respawnCount, mobsKilled, roomsCleared, bossesDefeated, powerUpsObtained;
    [HideInInspector]
    public List<GameObject> singletons = new List<GameObject>();

    int playerMoney, gameLevel, maxGameLevel;

    Transform miniMap;
    Transform pauseDisplay, pauseOptions, pauseSelector;
    Transform deathDisplay, deathOptions, deathSelector;
    Transform victoryDisplay, victoryOptions;
    int pauseSelectNum, deathSelectNum;
    bool menuOpen, selectBarFading;
    float selectBarFadeTimer;
    Vector3 mmOriginal;

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
        singletons.Add(this.gameObject);

        // Init
        playerMoney = 0;
        gameLevel = 1;
        maxGameLevel = 3;

        gameplayCanvas = GameObject.Find("Gameplay_Canvas").transform;
        miniMap = gameplayCanvas.GetChild(0).transform;
        mmOriginal = miniMap.localPosition;
        uiCanvas = GameObject.Find("UI_Canvas").transform;

        bossHPBar = uiCanvas.Find("BossHP");
        bossHPBar.gameObject.SetActive(false);

        pausePanel = uiCanvas.Find("Pause_Panel").gameObject;
        pauseDisplay = pausePanel.transform.Find("Display");
        pauseOptions = pausePanel.transform.Find("Options");
        pauseSelector = pauseOptions.Find("Selector");
        pausePanel.SetActive(false);

        deathPanel = uiCanvas.Find("Death_Panel").gameObject;
        deathDisplay = deathPanel.transform.Find("Display");
        deathOptions = deathPanel.transform.Find("Options");
        deathSelector = deathOptions.Find("Selector");
        deathPanel.SetActive(false);

        victoryPanel = uiCanvas.Find("Victory_Panel").gameObject;
        victoryDisplay = victoryPanel.transform.Find("Display");
        victoryOptions = victoryPanel.transform.Find("Options");
        victoryPanel.SetActive(false);

        pauseSelectNum = deathSelectNum = 0;

        menuOpen = false;
        selectBarFading = true;
        selectBarFadeTimer = 0;

        respawnCount = 3;
    }
    
	// Update is called once per frame
	void Update ()
    {
        UI_Input();
        UI_Update();

        //DebugMode();
	}

    public void ClearGameplaySingletons()
    {
        foreach (GameObject singleton in singletons)
            Destroy(singleton);
    }

    public int GetMoney()
    {
        return playerMoney;
    }

    public void ModMoney(int value)
    {
        playerMoney += value;
        uiCanvas.Find("PlayerUI").Find("Currency").gameObject.GetComponentInChildren<TextMeshProUGUI>().text = playerMoney.ToString();
    }

    public int GetCurrentLevel()
    {
        return gameLevel;
    }

    public void StartNextLevel()
    {
        StartCoroutine(NextLevel());
    }

    IEnumerator NextLevel()
    {
        if (gameLevel < maxGameLevel)
        {
            gameLevel++;
            // Disable Player momentarily
            PlayerCharacter.instance.transform.position = new Vector3(0, -GetComponent<SheetAssigner>().verticalOffset / 5.4f, 0);
            PlayerCharacter.instance.GetComponent<SpriteRenderer>().enabled = false;
            PlayerCharacter.instance.ToggleActive(false);

            // Erase rooms of current level
            for (int i = 0; i < gameplayCanvas.GetChild(1).childCount; i++)
            {
                Destroy(gameplayCanvas.GetChild(1).GetChild(i).gameObject);
            }

            // Move rooms back to starting position
            gameplayCanvas.GetChild(1).transform.localPosition = new Vector3(0, 0, 0);

            yield return new WaitForSeconds(1.0f);

            // Generate new rooms
            GetComponent<LevelGeneration>().GenerateNextLevel();

            // Re-enable the Player
            PlayerCharacter.instance.GetComponent<SpriteRenderer>().enabled = true;
            PlayerCharacter.instance.ToggleActive(true);
        }
        else if (gameLevel == maxGameLevel)
        {
            GameData.instance.UpdateData("totalRunsCompleted", GameData.instance.totalRunsCompleted+1);
            OpenVictoryMenu();
        }
    }

    void UI_Update()
    {
        if(menuOpen)
        {
            #region manual select bar blinking animation
            selectBarFadeTimer += Time.unscaledDeltaTime;
            if (selectBarFadeTimer >= 0.5f)
            {
                selectBarFadeTimer = 0;
                if (selectBarFading)
                    selectBarFading = false;
                else
                    selectBarFading = true;
            }

            if (pauseSelector.gameObject.activeSelf)
            {
                if (selectBarFading)
                {
                    Color newColor = new Color(pauseSelector.GetComponent<Image>().color.r, pauseSelector.GetComponent<Image>().color.g, pauseSelector.GetComponent<Image>().color.b, pauseSelector.GetComponent<Image>().color.a - (0.6f/30));
                    if (newColor.a < 0)
                        newColor.a = 0;
                    pauseSelector.GetComponent<Image>().color = newColor;
                }
                else
                {
                    Color newColor = new Color(pauseSelector.GetComponent<Image>().color.r, pauseSelector.GetComponent<Image>().color.g, pauseSelector.GetComponent<Image>().color.b, pauseSelector.GetComponent<Image>().color.a + (0.6f/30));
                    if (newColor.a > 0.6f)
                        newColor.a = 0.6f;
                    pauseSelector.GetComponent<Image>().color = newColor;
                }
            }
            if (deathSelector.gameObject.activeSelf)
            {
                if (selectBarFading)
                {
                    Color newColor = new Color(deathSelector.GetComponent<Image>().color.r, deathSelector.GetComponent<Image>().color.g, deathSelector.GetComponent<Image>().color.b, deathSelector.GetComponent<Image>().color.a - (0.6f / 30));
                    if (newColor.a < 0)
                        newColor.a = 0;
                    deathSelector.GetComponent<Image>().color = newColor;
                }
                else
                {
                    Color newColor = new Color(deathSelector.GetComponent<Image>().color.r, deathSelector.GetComponent<Image>().color.g, deathSelector.GetComponent<Image>().color.b, deathSelector.GetComponent<Image>().color.a + (0.6f / 30));
                    if (newColor.a > 0.6f)
                        newColor.a = 0.6f;
                    deathSelector.GetComponent<Image>().color = newColor;
                }
            }
            #endregion
        }
    }

    void UI_Input()
    {
        MinimapInput();
        PauseInput();
        RespawnMenuInput();
        VictoryMenuInput();
    }

    void MinimapInput()
    {
        #region minimap toggle
        if (Input.GetKeyDown(GameData.instance.miniMap))
        {
            miniMap.localPosition = Vector3.zero;
            miniMap.localScale *= 4;
        }
        else if (Input.GetKeyUp(GameData.instance.miniMap))
        {
            miniMap.localPosition = mmOriginal;
            miniMap.localScale /= 4;
        }
        #endregion
    }

    void PauseGame(bool pause)
    {
        // Pause all entities
        PlayerCharacter.instance.ToggleActive(!pause);
        foreach (GameObject mob in currentRoom.mobList)
        {
            mob.GetComponent<Entity>().ToggleActive(!pause);
        }

        if (pause)
        {
            menuOpen = true;
            Time.timeScale = 0;
            // Stat update
            pauseDisplay.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Damage: " + PlayerCharacter.instance.GetDamage();
            pauseDisplay.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Movement Speed: " + PlayerCharacter.instance.GetSpeed();
            pauseDisplay.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text = "Fire Rate: " + PlayerCharacter.instance.GetFireRate();
            pauseDisplay.GetChild(1).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Bullet Speed: " + PlayerCharacter.instance.GetProjectileSpeed();
        }
        else
        {
            menuOpen = false;
            Time.timeScale = 1;
        }

        // Toggle the menu
        pausePanel.SetActive(pause);
    }

    void PauseInput()
    {
        #region Pause Game
        // Controls
        if (pausePanel.activeSelf)
        {
            #region Menu Selection
            if (Input.GetKeyDown(GameData.instance.playerKeys.down))
            {
                if (pauseSelectNum == 1)
                    pauseSelectNum = 0;
                else
                    pauseSelectNum++;

                if (pauseSelectNum == 0)
                    pauseSelector.localPosition = new Vector3(pauseSelector.localPosition.x, 60, 0);
                else
                    pauseSelector.localPosition = new Vector3(pauseSelector.localPosition.x, -40, 0);
            }
            if (Input.GetKeyDown(GameData.instance.playerKeys.up))
            {
                if (pauseSelectNum == 0)
                    pauseSelectNum = 1;
                else
                    pauseSelectNum--;

                if (pauseSelectNum == 0)
                    pauseSelector.localPosition = new Vector3(pauseSelector.localPosition.x, 60, 0);
                else
                    pauseSelector.localPosition = new Vector3(pauseSelector.localPosition.x, -40, 0);
            }
            #endregion

            #region Buttons
            switch (pauseSelectNum)
            {
                case 0: // Resume
                    if (Input.GetKeyDown(GameData.instance.interact))
                        PauseGame(false);
                    break;
                case 1: // Quit
                    if (Input.GetKeyDown(GameData.instance.interact))
                    {
                        GameData.instance.UpdateData("totalRunsQuit", GameData.instance.totalRunsQuit+1);
                        HelperFunctions.SceneTransition("MainMenu");
                        ClearGameplaySingletons();
                    }
                    break;
            }
            #endregion
        }
        // Toggle menu on
        else { if (Input.GetKeyDown(GameData.instance.pauseOpen) && !menuOpen) PauseGame(true); }
        #endregion
    }

    public void OpenRespawnMenu()
    {
        PlayerCharacter.instance.GetComponent<SpriteRenderer>().enabled = false;
        gameplayCanvas.GetChild(1).transform.localPosition = new Vector3(10000, 10000, 0);

        // Toggle the menu
        deathPanel.SetActive(true);
        menuOpen = true;

        // Stat update
        deathDisplay.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Kills: " + mobsKilled;
        deathDisplay.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Rooms Cleared: " + roomsCleared;
        deathDisplay.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text = "Bosses Defeated: " + bossesDefeated;
        deathDisplay.GetChild(1).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Power Ups: " + powerUpsObtained;
        deathOptions.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Respawn: " + respawnCount;
        if (respawnCount <= 0)
        {
            GameData.instance.UpdateData("totalRunsFailed", GameData.instance.totalRunsFailed+1);
            deathSelectNum = 1;
            deathSelector.localPosition = new Vector3(deathSelector.localPosition.x, deathSelector.localPosition.y + 100 - (200 * deathSelectNum), 0);
            deathOptions.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.red;
        }
    }

    void RespawnMenuInput()
    {
        // Controls
        if (deathPanel.activeSelf)
        {
            #region Menu Selection
            if (Input.GetKeyDown(GameData.instance.playerKeys.down))
            {
                if (deathSelectNum == 1)
                    deathSelectNum = 0;
                else
                    deathSelectNum++;

                if (respawnCount <= 0)
                {
                    deathSelectNum = 1;
                }

                if (deathSelectNum == 0)
                    deathSelector.localPosition = new Vector3(deathSelector.localPosition.x, 60, 0);
                else
                    deathSelector.localPosition = new Vector3(deathSelector.localPosition.x, -40, 0);
            }
            if (Input.GetKeyDown(GameData.instance.playerKeys.up))
            {                
                if (deathSelectNum == 0)
                    deathSelectNum = 1;
                else
                    deathSelectNum--;

                if (respawnCount <= 0)
                {
                    deathSelectNum = 1;
                }

                if (deathSelectNum == 0)
                    deathSelector.localPosition = new Vector3(deathSelector.localPosition.x, 60, 0);
                else
                    deathSelector.localPosition = new Vector3(deathSelector.localPosition.x, -40, 0);
            }
            #endregion

            #region Buttons
            switch (deathSelectNum)
            {
                case 0: // Resume
                    if (Input.GetKeyDown(GameData.instance.interact) && respawnCount > 0)
                        StartCoroutine(Respawn());
                    break;
                case 1: // Quit
                    if (Input.GetKeyDown(GameData.instance.interact))
                    {
                        if (respawnCount > 0)
                            GameData.instance.UpdateData("totalRunsQuit", GameData.instance.totalRunsQuit+1);

                        HelperFunctions.SceneTransition("MainMenu");
                        ClearGameplaySingletons();
                    }
                    break;
            }
            #endregion
        }
    }

    IEnumerator Respawn()
    {
        respawnCount--;
        menuOpen = false;
        deathPanel.SetActive(false);

        foreach (RoomInstance room in GetComponent<SheetAssigner>().roomList)
            room.RefreshRooms();

        GetComponent<LevelGeneration>().ClearMap();
        GetComponent<LevelGeneration>().DrawMap();

        yield return new WaitForSeconds(1.0f);

        // Give power up to enemy in room that player died in, if player has any power ups
        if (PlayerCharacter.instance.GetPowerUps().Count > 0)
        {
            int rand = Random.Range(0, currentRoom.mobList.Count);
            PowerUp lostPower = PlayerCharacter.instance.RemoveRandPowerUp();
            Debug.Log("currentRoom: " + currentRoom.name + "\nMobList: " + currentRoom.mobList.Count);
            currentRoom.mobList[rand].GetComponent<Entity>().AddPowerUp(lostPower);
        }

        // Move rooms back to starting position
        gameplayCanvas.GetChild(1).transform.localPosition = new Vector3(0, 0, 0);

        // Respawn the player
        PlayerCharacter.instance.transform.position = new Vector3(0, -GetComponent<SheetAssigner>().verticalOffset/5.4f, 0);
        PlayerCharacter.instance.GetComponent<SpriteRenderer>().enabled = true;
        PlayerCharacter.instance.InitializePlayer();
        PlayerCharacter.instance.RefreshPowerUp();

        foreach (RoomInstance room in GetComponent<SheetAssigner>().roomList)
            room.RefreshRoomCleared();
    }

    public void OpenVictoryMenu()
    {
        PlayerCharacter.instance.GetComponent<SpriteRenderer>().enabled = false;
        gameplayCanvas.GetChild(1).transform.localPosition = new Vector3(10000, 10000, 0);

        // Toggle the menu
        victoryPanel.SetActive(true);
        menuOpen = true;

        // Stat update
        victoryDisplay.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Damage: " + PlayerCharacter.instance.GetDamage();
        victoryDisplay.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Movement Speed: " + PlayerCharacter.instance.GetSpeed();
        victoryDisplay.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text = "Fire Rate: " + PlayerCharacter.instance.GetFireRate();
        victoryDisplay.GetChild(1).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Bullet Speed: " + PlayerCharacter.instance.GetProjectileSpeed();
        victoryDisplay.GetChild(1).GetChild(4).GetComponent<TextMeshProUGUI>().text = "Kills: " + mobsKilled;
        victoryDisplay.GetChild(1).GetChild(5).GetComponent<TextMeshProUGUI>().text = "Rooms Cleared: " + roomsCleared;
        victoryDisplay.GetChild(1).GetChild(6).GetComponent<TextMeshProUGUI>().text = "Bosses Defeated: " + bossesDefeated;
        victoryDisplay.GetChild(1).GetChild(7).GetComponent<TextMeshProUGUI>().text = "Power Ups: " + powerUpsObtained;
        victoryOptions.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Press " + GameData.instance.interact.ToString() + " to return to menu";
    }

    void VictoryMenuInput()
    {
        if (victoryPanel.activeSelf)
        {
            if (Input.GetKeyDown(GameData.instance.interact))
            {
                HelperFunctions.SceneTransition("MainMenu");
                ClearGameplaySingletons();
            }
        }
    }
}
