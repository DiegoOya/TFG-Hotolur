using UnityEngine;

/// <summary>
/// Script to manage the interaction between the player and the objects with this script
/// </summary>
public class Interactable : MonoBehaviour {

    // Interaction radious, if the player is inside this radious then they can interact
    public float radius = 1f;

    // If you prefer you can assign another transform to calculate the radious
    public Transform interactionTransform;

    protected float distance;

    // Transform of the player and protected so it can be extended to child classes
    protected Transform player;

    // Initialize player
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
    }

    private void Update()
    {
        // Calculate the distance between the player and the interactable
        distance = Mathf.Sqrt(
            (player.position.x - interactionTransform.position.x) * (player.position.x - interactionTransform.position.x) +
            (player.position.y - interactionTransform.position.y) * (player.position.y - interactionTransform.position.y));

        // If it is inside the radious interact
        if (distance <= radius)
        {
            Interact();
        }
    }

    // Called when interacting
    public virtual void Interact()
    {
        // This method is going to be overwritten 
        Debug.Log("Interacting with " + transform.name);
    }

    // To give a visual example of the radious of the interactable
    private void OnDrawGizmosSelected()
    {
        if (interactionTransform == null)
            interactionTransform = transform;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }
}
