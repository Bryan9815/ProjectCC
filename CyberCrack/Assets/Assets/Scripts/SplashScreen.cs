using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    void GoToMainMenu()
    {
        StartCoroutine(HelperFunctions.SceneTransition(0.5f, "MainMenu"));
    }    
}
