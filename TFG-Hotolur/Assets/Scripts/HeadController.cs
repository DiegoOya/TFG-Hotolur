using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Script used to control the head
/// </summary>
public class HeadController : MonoBehaviour {

    [HideInInspector]
    public float time;

    // Sounds of the head when the player dies next to him or by him
    public AudioClip audioNextToHim;
    public AudioClip audioDiesByHim;
    
    private float sizeCamX;
    [SerializeField]
    private float velocityX = 2f;
    [SerializeField]
    private float penaltyRate = 2f;
    private float timeToNextPenalty;
    [SerializeField]
    private float volumeSounds = 0.7f;

    private bool playerDied = false;
    private bool headStopped = false;
    private bool headLaughed = false;

    private Rigidbody rb;

    private Transform player;
    private Transform cam;

    private PlayerHealth playerHealth;

    private TextMeshProUGUI timeText;

    // Initialize references
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(velocityX, 0f, 0f);

        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        
        cam = Camera.main.transform;
        sizeCamX = Camera.main.orthographicSize * 16 / 9;

        timeToNextPenalty = 1f / penaltyRate;
    }

    private void Update()
    {
        // Look at what position is the player in the Y axis and move the head 
        if (!headStopped)
        {
            float posY = player.position.y < 5.5f ? 5.5f : player.position.y;
            posY = Mathf.Lerp(transform.position.y, posY, 1.5f * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, posY, transform.position.z);
        }

        // Calculate the time needed until the head reaches the player and show it in the canvas
        float distance = Vector3.Magnitude(player.position - transform.position);
        time = distance / velocityX;

        GameObject timeTextGO = GameObject.FindGameObjectWithTag(Tags.timeCounterText);
        if (timeTextGO != null)
        {
            timeText = timeTextGO.GetComponent<TextMeshProUGUI>();
            if (playerDied && !headLaughed)
            {
                AudioSource.PlayClipAtPoint(audioDiesByHim, transform.position, volumeSounds);
                timeText.text = "GOT YA!!";
                headLaughed = true;
            }
            else
                timeText.text = string.Concat("La Criatura-Hotolur: ", (float)(Mathf.Floor(time * 10) / 10), " s");
        }

        // Get the position of the left side of the camera and if the position of the head is
        // greater than the position of the left side the hurt the player
        float posCameraSide = cam.position.x - sizeCamX; 
        if (transform.position.x + 5f > posCameraSide)
        {
            if (timeToNextPenalty < 0 && playerHealth.GetHealth() > 0)
            {
                playerHealth.PenaltyDamage(2f);
                timeToNextPenalty = 1f / penaltyRate;
            }

            if (playerHealth.GetHealth() <= 0 && !headLaughed)
            {
                AudioSource.PlayClipAtPoint(audioNextToHim, transform.position, volumeSounds);
                headLaughed = true;
            }
        }

        timeToNextPenalty -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the head touches the player then kill the player
        if(other.CompareTag(Tags.player) && playerHealth.GetHealth() > 0)
        {
            playerHealth.PenaltyDamage(playerHealth.maxHealth);
            playerDied = true;
        }
    }

    // When the player pick up a watch then stops the player
    public void StopHead(float stopTime)
    {
        rb.velocity = Vector3.zero;
        headStopped = true;
        StartCoroutine(ReactivateHead(stopTime));
    }

    // Start moving the head again after stopTime seconds
    private IEnumerator ReactivateHead(float stopTime)
    {
        yield return new WaitForSeconds(stopTime);

        rb.velocity = new Vector3(velocityX, 0f, 0f);
        headStopped = false;
    }

}
