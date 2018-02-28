using UnityEngine;

public class Interactable : MonoBehaviour {

    public float radius = 1f;

    public Transform interactionTransform;
    protected Transform player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
    }

    public virtual void Interact()
    {
        // This method is going to be overwritten 
        Debug.Log("Interacting with " + transform.name);
    }

    private void Update()
    {
        float distance = Mathf.Sqrt(
            (player.position.x - interactionTransform.position.x) * (player.position.x - interactionTransform.position.x) +
            (player.position.y - interactionTransform.position.y) * (player.position.y - interactionTransform.position.y));

        if (distance <= radius)
        {
            Interact();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (interactionTransform == null)
            interactionTransform = transform;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }
}
