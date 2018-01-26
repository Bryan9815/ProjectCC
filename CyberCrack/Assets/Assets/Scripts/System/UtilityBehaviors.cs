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
            foreach (GameObject singleton in GameController.instance.singletons)
                Destroy(singleton);

            //reload scene, for testing purposes
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
	}
}
