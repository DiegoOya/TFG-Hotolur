using UnityEngine;

/// <summary>
/// Script to control when the player enters a checkpoint
/// </summary>
public class Checkpoint : MonoBehaviour {

    private bool hasEntered = false;

    private void OnTriggerEnter(Collider other)
    {
        // If it is the player then save the game
        if(other.CompareTag(Tags.player) && !hasEntered)
        {
            GameController.instance.CheckpointEntered(transform.position);
            GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerHealth>().HealPlayer(100);
            hasEntered = true;
        }
    }

    // Setter of hasEntered
    public void SetHasEntered(bool entered)
    {
        hasEntered = entered;
    }

}
