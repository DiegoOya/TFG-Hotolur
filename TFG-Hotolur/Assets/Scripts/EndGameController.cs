using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Script to control when the player enters the finish platform
/// </summary>
public class EndGameController : MonoBehaviour {

    [SerializeField]
    private float factorPoints = 10f;

    private GameObject endPanel;

    private TextMeshProUGUI winText;
    private TMP_InputField nameIF;

    private HeadController headController;
    
    private void Start()
    {
        // Search for the endPanel and deactivate it
        endPanel = GameObject.FindGameObjectWithTag(Tags.endPanel);
        winText = endPanel.GetComponentInChildren<TextMeshProUGUI>();
        nameIF = endPanel.GetComponentInChildren<TMP_InputField>();
        nameIF.enabled = false;
        endPanel.SetActive(false);

        headController = GameObject.FindGameObjectWithTag(Tags.head).GetComponent<HeadController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Tags.player))
        {
            // When the player finishes the game add a bonus proportional to the time between head and player
            float bonus = headController.time * factorPoints;
            GameController.instance.UpdatePoints(Mathf.FloorToInt(bonus));

            endPanel.SetActive(true);
            GameController.instance.doingSetup = true;
            headController.StopHead(20f);

            if (GameController.instance.points >= GameController.instance.pointsToWin)
            {
                winText.text = string.Concat("¡Has ganado! \nLos puntos obtenidos han sido: ", GameController.instance.points.ToString(), 
                    "\nCon un tiempo: ", headController.time.ToString(), "\n\n  Escribe tu nombre (máx. 3 caracteres): ");
                nameIF.enabled = true;
            }
            else
            {
                winText.text = string.Concat("¡Oooh! No has coseguido los puntos necesarios \nLos puntos obtenidos han sido: ", GameController.instance.points.ToString(),
                   "\nCon un tiempo: ", headController.time.ToString(), "\n\n  Suerte en el próximo intento");

                StartCoroutine(LoadMenu());
            }
        }
    }

    private IEnumerator LoadMenu()
    {
        yield return new WaitForSeconds(5f);

        Time.timeScale = 1f;
        GameController.instance.NewGame(0);
    }

}
