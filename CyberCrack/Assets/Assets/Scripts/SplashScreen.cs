using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    void GoToMainMenu()
    {
        StartCoroutine(HelperFunctions.DelayedSceneTransition("MainMenu", 0.5f));
    }    
}
