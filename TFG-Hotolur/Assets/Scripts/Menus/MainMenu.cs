using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script used to manage the main menu
/// </summary>
public class MainMenu : MonoBehaviour {

    // Text to show when there is no game saved
    [HideInInspector]
    public TextMeshProUGUI noGameSavedText;

    private void Start()
    {
        GameController.instance.UpdateRanking();
    }

    // When pushed the New Game button in the menu, access to a new game
    public void NewGame()
    {
        GameController.instance.doingSetup = true;
        GameController.instance.NewGame(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Load the last play saved
    public void LoadGame()
    {
        if(!GameController.instance.LoadGame())
        {
            GameController.instance.doingSetup = true;
            noGameSavedText.enabled = true;
            StartCoroutine(DeactivateText());
        }
    }

    // When pushed the Quit button in the menu, quit the game
    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator DeactivateText()
    {
        yield return new WaitForSeconds(2f);

        noGameSavedText.enabled = false;
    }

}
