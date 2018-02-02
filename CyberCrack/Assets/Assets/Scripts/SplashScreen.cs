using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    private void Start()
    {
        GameData.instance.UpdateWindow(BayatGames.SaveGameFree.SaveGame.Load("resolution", 5), BayatGames.SaveGameFree.SaveGame.Load("windowMode", 0));
    }

    void GoToMainMenu()
    {
        StartCoroutine(HelperFunctions.DelayedSceneTransition("MainMenu", 0.5f));
    }    
}
