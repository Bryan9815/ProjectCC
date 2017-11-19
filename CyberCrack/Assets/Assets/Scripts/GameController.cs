using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    Transform gameplayCanvas;
    GameObject PlayerCharacter;
	// Use this for initialization
	void Start ()
    {
        gameplayCanvas = transform.GetChild(1);
        PlayerCharacter = gameplayCanvas.GetChild(1).gameObject;
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
                    // Open menu, secondary input for which testDummy to spawn
                    GameObject testDummy = Instantiate(Resources.Load<GameObject>("Prefabs/TestDummy"), gameplayCanvas);
                    break;
                case "p":
                    // Gives/Spawns power up
                    PlayerCharacter.GetComponent<PlayerCharacter>().ActivateTripleShot();
                    break;
                case "u":
                    // Give/Spawns usable item
                    break;
                case "h":
                    // Gives/Spawns health
                    break;
            }
        }
    }
}
