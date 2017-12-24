using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public Transform gameplayCanvas;
    public Transform uiCanvas;
    public GameObject debugEnemyPanel;
    public GameObject playerCharacter;

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
    }

    // Use this for initialization
    void Start ()
    {
        gameplayCanvas = GameObject.Find("Gameplay_Canvas").transform;
        uiCanvas = GameObject.Find("UI_Canvas").transform;

        debugEnemyPanel = uiCanvas.GetChild(1).gameObject;
        playerCharacter = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update ()
    {
        DebugMode();
	}

    void DebugMode()
    {
        if(Input.GetKey(KeyCode.Tab))
        {
            string input = Input.inputString;

            if (input != "")
                Debug.Log(input);

            switch(input)
            {
                case "g":
                   debugEnemyPanel.SetActive(true);
                    break;
                case "p":
                    // Gives/Spawns power up
                    playerCharacter.GetComponent<PlayerCharacter>().ActivateTripleShot();
                    break;
                case "u":
                    // Give/Spawns usable item
                    break;
                case "h":
                    // Gives/Spawns health
                    break;
            }
        }

        if (debugEnemyPanel.activeSelf)
        {
            string input = Input.inputString;

            if (input != "")
                Debug.Log(input);

            switch (input)
            {
                case "1":
                    GameObject testDummy = Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/TestDummy"), gameplayCanvas);
                    debugEnemyPanel.SetActive(false);
                    break;
                case "2":
                    GameObject chaser = Instantiate(Resources.Load <GameObject> ("Prefabs/Enemies/Chaser"), gameplayCanvas);
                    debugEnemyPanel.SetActive(false);
                    break;
                case "3":
                    GameObject burst = Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/Burst"), gameplayCanvas);
                    debugEnemyPanel.SetActive(false);
                    break;
                case "4":
                    GameObject continuous = Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/Continuous"), gameplayCanvas);
                    debugEnemyPanel.SetActive(false);
                    break;
            }

            if (Input.GetKey(KeyCode.Escape))
            {
                debugEnemyPanel.SetActive(false);
            }
        }
    }
}
