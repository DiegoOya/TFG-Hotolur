using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script used to manage the pause menu
/// </summary>
public class PauseMenu : MonoBehaviour {

    // Variable to control when the game is paused or not
    public static bool gameIsPaused = false;

    // GameObject of the pause menu
    public GameObject pauseMenuUI;

    private void Update()
    {   
        // If the escape button is pressed and the game is active, then pause, and backwards
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
                Resume();
            else
            {
                Pause();
            }
        }
    }

    // When pushed the resume button or the escape button, then resume the game
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        GameController.instance.doingSetup = false;
    }

    // Pause the game
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        GameController.instance.doingSetup = true;
    }

    // Load the main menu scene
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        GameController.instance.NewGame(0);
    } 

    // When the quit button is pushed then quit the game
    public void QuitGame()
    {
        Application.Quit();
    }

}
