using UnityEngine;

/// <summary>
/// Script to control when the player enters a checkpoint
/// </summary>
public class Checkpoint : MonoBehaviour {

    // Audio when the player enters the checkpoint
    public AudioClip audioCheckpoint;

    [SerializeField]
    private float volumeSounds = 0.7f;

    private bool hasEntered = false;

    private void OnTriggerEnter(Collider other)
    {
        // If it is the player then save the game
        if(other.CompareTag(Tags.player) && !hasEntered)
        {
            GameController.instance.CheckpointEntered(transform.position);
            GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerHealth>().HealPlayer(100);
            AudioSource.PlayClipAtPoint(audioCheckpoint, transform.position, volumeSounds);
            hasEntered = true;
        }
    }

    // Setter of hasEntered
    public void SetHasEntered(bool entered)
    {
        hasEntered = entered;
    }

}
