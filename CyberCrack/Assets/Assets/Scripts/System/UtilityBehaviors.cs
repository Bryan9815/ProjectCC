using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UtilityBehaviors : MonoBehaviour
{
	void Update ()
    {
        if (Input.GetKeyDown("r"))
        {
            Debug.Log("Restarting game");
            foreach (GameObject singleton in GameController.instance.singletons)
                Destroy(singleton);

            //reload scene, for testing purposes
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if(Input.GetKeyDown("g"))
        {
            GameController.instance.StartNextLevel();
        }
        if(Input.GetKeyDown("c"))
        {
            PlayerCharacter.instance.ModifyPlayerDamage(+50);
            PlayerCharacter.instance.ModifyMaxHP(+10);
            PlayerCharacter.instance.ModifyHP(+10);
            PlayerCharacter.instance.ModifySpeed(+1);
            PlayerCharacter.instance.ModifyFireRate(+15);
        }
        if(Input.GetKeyDown("j"))
        {
            Debug.Log("Player hp increased");
            PlayerCharacter.instance.ModifyHP(+1);
        }
    }
}
