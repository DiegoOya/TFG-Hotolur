using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script used to manage the main menu
/// </summary>
public class MainMenu : MonoBehaviour {

    // When pushed the Play button in the menu, access to the game
	public void PlayGame()
    {
        //********* MAYBE DO A LITTLE MORE LIKE SELECT MULTIPLE SAVE GAMES
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // When pushed the Quit button in the menu, quit the game
    public void QuitGame()
    {
        Application.Quit();
    }

}
