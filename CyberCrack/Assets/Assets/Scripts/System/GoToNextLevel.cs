using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToNextLevel : MonoBehaviour
{
    public static void CreateDoorToNextLevel(Transform parent)
    {
        Instantiate(Resources.Load("Prefabs/NextLevelDoor"), parent);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GameController.instance.StartNextLevel();
        }
    }
}
