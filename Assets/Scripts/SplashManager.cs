using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashManager : MonoBehaviour
{
    [Header("Scene Names")]
    public string arSceneName = "ARScene";
    public string mainMenuSceneName = "MainMenu";

    [Header("Settings")]
    public float delayBeforeLoad = 1f;

    private void Start()
    {
        // Set orientation and load scene based on platform after a short delay
        Invoke(nameof(LoadNextScene), delayBeforeLoad);
    }

    private void LoadNextScene()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        // Android Specific Logic
        Debug.Log("Platform: Android. Setting Portrait and loading ARScene.");
        Screen.orientation = ScreenOrientation.Portrait;
        SceneManager.LoadScene(arSceneName);
#else
        // Windows / Editor Specific Logic
        Debug.Log("Platform: Windows/Editor. Setting Landscape and loading MainMenu.");
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        SceneManager.LoadScene(mainMenuSceneName);
#endif
    }
}
