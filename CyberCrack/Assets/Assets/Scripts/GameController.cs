using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public Transform gameplayCanvas;
    public Transform uiCanvas;
    [HideInInspector]
    public Transform bossHPBar;
    [HideInInspector]
    public GameObject pausePanel;
    [HideInInspector]
    public GameObject deathPanel;
    [HideInInspector]
    public RoomInstance currentRoom;
    [HideInInspector]
    public int respawnCount, mobsKilled, roomsCleared, bossesDefeated, powerUpsObtained;
    [HideInInspector]
    public List<GameObject> singletons = new List<GameObject>();

    Transform miniMap;
    Transform pauseDisplay, pauseOptions, pauseSelector;
    Transform deathDisplay, deathOptions, deathSelector;
    int pauseSelectNum, deathSelectNum;
    bool menuOpen;
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

        pauseSelectNum = deathSelectNum = 0;

        menuOpen = false;

        respawnCount = 3;
    }

    // Use this for initialization
    void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        UI_Input();
        UI_Update();

        //DebugMode();
	}

    void UI_Update()
    {

    }

    void UI_Input()
    {
        #region debug
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(Respawn());
        }
        #endregion

        MinimapInput();
        PauseInput();
        RespawnMenuInput();
    }

    void MinimapInput()
    {
        #region minimap toggle
        if (Input.GetKeyDown(KeyCode.M))
        {
            miniMap.localPosition = Vector3.zero;
            miniMap.localScale *= 4;
        }
        else if (Input.GetKeyUp(KeyCode.M))
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
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                if (pauseSelectNum < 2)
                {
                    pauseSelectNum++;
                    pauseSelector.transform.localPosition += new Vector3(0, -90, 0);
                }
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                if (pauseSelectNum > 0)
                {
                    pauseSelectNum--;
                    pauseSelector.transform.localPosition += new Vector3(0, 90, 0);
                }
            }
            #endregion

            #region Buttons
            switch (pauseSelectNum)
            {
                case 0: // Resume
                    if (Input.GetKeyDown(KeyCode.Space))
                        PauseGame(false);
                    break;
                case 1: // Power Ups
                    if (Input.GetKeyDown(KeyCode.Space))
                        break;
                    break;
                case 2: // Quit
                    if (Input.GetKeyDown(KeyCode.Space))
                        Application.Quit();
                    break;
            }
            #endregion
        }
        // Toggle menu on
        else { if (Input.GetKeyDown(KeyCode.Escape) && !menuOpen) PauseGame(true); }
        #endregion
    }

    public void OpenRespawnMenu()
    {
        PlayerCharacter.instance.GetComponent<Renderer>().enabled = false;
        gameplayCanvas.GetChild(1).transform.localPosition = new Vector3(10000, 10000, 0);

        // Toggle the menu
        deathPanel.SetActive(true);
        menuOpen = true;

        // Stat update
        deathDisplay.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Kills: ";
        deathDisplay.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Rooms Cleared: ";
        deathDisplay.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text = "Bosses Defeated: ";
        deathDisplay.GetChild(1).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Power Ups: ";
        deathOptions.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Respawn: " + respawnCount;
        if (respawnCount <= 0)
        {
            deathSelectNum = 1;
            deathSelector.transform.localPosition += new Vector3(0, -90, 0);
            deathOptions.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.red;
        }
    }

    void RespawnMenuInput()
    {
        // Controls
        if (deathPanel.activeSelf)
        {
            #region Menu Selection
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                if (deathSelectNum < 2)
                {
                    deathSelectNum++;
                    deathSelector.transform.localPosition += new Vector3(0, -90, 0);
                }
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                if (respawnCount > 0)
                {
                    if (deathSelectNum > 0)
                    {
                        deathSelectNum--;
                        deathSelector.transform.localPosition += new Vector3(0, 90, 0);
                    }
                }
                else
                {
                    if (deathSelectNum > 1)
                    {
                        deathSelectNum--;
                        deathSelector.transform.localPosition += new Vector3(0, 90, 0);
                    }
                }
            }
            #endregion

            #region Buttons
            switch (deathSelectNum)
            {
                case 0: // Resume
                    if (Input.GetKeyDown(KeyCode.Space) && respawnCount > 0)
                        StartCoroutine(Respawn());
                    break;
                case 1: // Power Ups
                    if (Input.GetKeyDown(KeyCode.Space))
                        break;
                    break;
                case 2: // Quit
                    if (Input.GetKeyDown(KeyCode.Space))
                        Application.Quit();
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
        {
            room.RefreshRooms();
        }

        yield return new WaitForSeconds(1.0f);

        // Move rooms back to starting position
        gameplayCanvas.GetChild(1).transform.localPosition = new Vector3(0, 0, 0);
        // Give power up to enemy in room that player died in, if player has any power ups
        if(PlayerCharacter.instance.GetPowerUps().Count > 0)
        {
            int rand = Random.Range(0, currentRoom.mobList.Count);
            PowerUp lostPower = PlayerCharacter.instance.RemoveRandPowerUp();
            currentRoom.mobList[rand].GetComponent<Entity>().AddPowerUp(lostPower);
        }

        // Respawn the player
        PlayerCharacter.instance.transform.position = new Vector3(0, 0, 0);
        PlayerCharacter.instance.GetComponent<Renderer>().enabled = true;
        PlayerCharacter.instance.InitializePlayer();
        PlayerCharacter.instance.RefreshPowerUp();
    }
}
