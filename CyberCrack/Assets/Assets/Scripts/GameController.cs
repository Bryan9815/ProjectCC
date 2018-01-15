using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject playerCharacter;
    [HideInInspector]
    public RoomInstance currentRoom;

    Transform miniMap;
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

        // Init
        gameplayCanvas = GameObject.Find("Gameplay_Canvas").transform;
        miniMap = gameplayCanvas.GetChild(0).transform;
        mmOriginal = miniMap.localPosition;
        uiCanvas = GameObject.Find("UI_Canvas").transform;

        bossHPBar = uiCanvas.Find("BossHP");
        bossHPBar.gameObject.SetActive(false);

        pausePanel = uiCanvas.Find("Pause_Panel").gameObject;
        pausePanel.SetActive(false);
        playerCharacter = GameObject.FindGameObjectWithTag("Player");
    }

    // Use this for initialization
    void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(playerCharacter == null)
            playerCharacter = GameObject.FindGameObjectWithTag("Player");

        UI_Input();

        //DebugMode();
	}

    void UI_Input()
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
        #region Pause Game
        // Controls
        if (pausePanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) { PauseGame(false); }

        }
        // Toggle
        else { if (Input.GetKeyDown(KeyCode.Escape)) PauseGame(true); }
        #endregion
    }

    void PauseGame(bool pause)
    {
        // Pause all entities
        playerCharacter.GetComponent<Entity>().ToggleActive(!pause);
        foreach (GameObject mob in currentRoom.mobList)
        {
            mob.GetComponent<Entity>().ToggleActive(!pause);
        }

        if (pause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;

        // Toggle the menu
        pausePanel.SetActive(pause);
    }

    //void DebugMode()
    //{
    //    if(Input.GetKey(KeyCode.Tab))
    //    {
    //        string input = Input.inputString;

    //        if (input != "")
    //            Debug.Log(input);

    //        switch(input)
    //        {
    //            case "g":
    //               debugEnemyPanel.SetActive(true);
    //                break;
    //            case "p":
    //                // Gives/Spawns power up
    //                playerCharacter.GetComponent<PlayerCharacter>().ActivateTripleShot();
    //                break;
    //            case "u":
    //                // Give/Spawns usable item
    //                break;
    //            case "h":
    //                // Gives/Spawns health
    //                break;
    //        }
    //    }

    //    if (debugEnemyPanel.activeSelf)
    //    {
    //        string input = Input.inputString;

    //        if (input != "")
    //            Debug.Log(input);

    //        switch (input)
    //        {
    //            case "1":
    //                GameObject testDummy = Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/TestDummy"), gameplayCanvas);
    //                debugEnemyPanel.SetActive(false);
    //                break;
    //            case "2":
    //                GameObject chaser = Instantiate(Resources.Load <GameObject> ("Prefabs/Enemies/Chaser"), gameplayCanvas);
    //                debugEnemyPanel.SetActive(false);
    //                break;
    //            case "3":
    //                GameObject burst = Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/Burst"), gameplayCanvas);
    //                debugEnemyPanel.SetActive(false);
    //                break;
    //            case "4":
    //                GameObject continuous = Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/Continuous"), gameplayCanvas);
    //                debugEnemyPanel.SetActive(false);
    //                break;
    //        }

    //        if (Input.GetKey(KeyCode.Escape))
    //        {
    //            debugEnemyPanel.SetActive(false);
    //        }
    //    }
    //}
}
