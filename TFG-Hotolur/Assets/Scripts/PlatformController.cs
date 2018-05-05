using UnityEngine;

/// <summary>
/// Script used to control the collider of the platforms 
/// </summary>
public class PlatformController : MonoBehaviour {

    //private Vector3 player;

    //private void Start()
    //{
    //    player = GameObject.FindGameObjectWithTag(Tags.player).transform.position;
    //}

    //private void Update()
    //{
    //    // Determines whether isGrounded is true or false by seeing if the feet of the player overlaps with the platform
    //    bool isGrounded = Physics.CheckSphere(player - new Vector3(0f, 0.5f, 0f),
    //        0.2f, LayerMask.GetMask("Platform"));

    //    // If the player is on the platform activate the platform collider
    //    gameObject.GetComponent<Collider>().enabled = isGrounded;
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.player))
        {
            // Determines whether isGrounded is true or false by seeing if the feet of the player overlaps with the platform
            bool isGrounded = Physics.CheckSphere(other.gameObject.transform.position - new Vector3(0f, 0.5f, 0f),
                0.2f, LayerMask.GetMask("Ground"));

            // If the player is on the platform activate the platform collider
            gameObject.GetComponentInChildren<MeshCollider>().enabled = isGrounded;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.player))
        {
            // Determines whether isGrounded is true or false by seeing if the feet of the player overlaps with the platform
            bool isGrounded = Physics.CheckSphere(other.gameObject.transform.position - new Vector3(0f, 0.5f, 0f),
            0.2f, LayerMask.GetMask("Ground"));

            // If the player is on the platform deactivate the platform collider
            gameObject.GetComponentInChildren<MeshCollider>().enabled = isGrounded;
        }
    }

}
