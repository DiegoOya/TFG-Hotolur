using UnityEngine;

/// <summary>
/// Script to manage the camera movement
/// </summary>
public class CameraController : MonoBehaviour
{
    [HideInInspector]
    public float cameraVelX = 0f;

    // Reference of the player Transform
    private Transform player;

    // This will control the offset between the camera and the player
    private Vector3 offset;

    private Rigidbody rb;

    // Initialize the variables
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        rb = player.GetComponent<Rigidbody>();
        offset = transform.position - player.transform.position;
    }

    // It will be updated after the physics and update the camera position
    void LateUpdate()
    {
        float posY = player.position.y < 5f ? 5f : player.position.y;
        posY = Mathf.Lerp(transform.position.y, posY, 1.5f * Time.deltaTime);

        Vector3 newPos = new Vector3(player.position.x + offset.x, posY, transform.position.z);

        transform.position = newPos;

        cameraVelX = rb.velocity.x;
    }
}
