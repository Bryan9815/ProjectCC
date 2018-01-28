using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    void GoToMainMenu()
    {
        StartCoroutine(HelperFunctions.SceneTransition("MainMenu", 0.5f));
    }    
}
