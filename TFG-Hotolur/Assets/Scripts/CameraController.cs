using UnityEngine;

/// <summary>
/// Script to manage the camera movement
/// </summary>
public class CameraController : MonoBehaviour
{
    [HideInInspector]
    public float cameraVelX = 0f;

    // Reference of the player
    private GameObject player;

    // This will control the offset between the camera and the player
    private Vector3 offset;

    private Rigidbody rb;

    // Initialize the variables
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag(Tags.player);
        rb = player.GetComponent<Rigidbody>();
        offset = transform.position - player.transform.position;
    }

    // It will be updated after the physics and update the camera position
    void LateUpdate()
    {
        Vector3 newPos = new Vector3(player.transform.position.x + offset.x, transform.position.y, transform.position.z);

        transform.position = newPos;

        cameraVelX = rb.velocity.x;
    }
}
