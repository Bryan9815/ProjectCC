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
            GameController.instance.ClearGameplaySingletons();

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
            PlayerCharacter.instance.ModifyHP(+1);
        }
        if(Input.GetKeyDown("o"))
        {
            GameController.instance.ModMoney(+10);
        }
    }
}
