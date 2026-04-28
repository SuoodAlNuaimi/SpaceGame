using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleMenu : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject optionsPanel;
    public GameObject loading;
    
    // Simple start game
    public void StartGame()
    {
        loading.SetActive(true);
        Invoke("ChangeScene", 3f);
    }
    public void ChangeScene()
    {
        SceneManager.LoadScene("Exploration"); // Change to your game scene name

    }
    // Show options panel
    public void ShowOptions()
    {
        mainPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
    
    // Back to main menu
    public void BackToMain()
    {
        mainPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }
    
    // Quit the game
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    // Simple options functions
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
    }
    
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }
    
    void Start()
    {
        // Load saved volume
        float savedVolume = PlayerPrefs.GetFloat("Volume", 0.75f);
        AudioListener.volume = savedVolume;
        
        // Load saved fullscreen
        bool savedFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        Screen.fullScreen = savedFullscreen;
    }
}