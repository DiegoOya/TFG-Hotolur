using UnityEngine;

/// <summary>
/// Script to manage the camera movement
/// </summary>
public class CameraController : MonoBehaviour
{
    // Variables para marcar el rango de movimiento de la cámara en el eje x y el objetivo de la cámara
    //public GameObject ini;
    //public GameObject fin;

    [SerializeField]
    private float smoothing = 1f;

    // Reference of the player
    private GameObject player;

    // This will control the offset between the camera and the player
    private Vector3 offset;

    // Initialize the variables
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag(Tags.player);
        offset = transform.position - player.transform.position;
    }

    // It will be updated after the physics and update the camera position
    void LateUpdate()
    {
        Vector3 newPos = new Vector3(player.transform.position.x + offset.x, transform.position.y, transform.position.z);

        transform.position = newPos;
        //transform.position = new Vector3(Mathf.Clamp(transform.position.x, ini.transform.position.x + 26, fin.transform.position.x - 26), transform.position.y, transform.position.z);
    }
}
