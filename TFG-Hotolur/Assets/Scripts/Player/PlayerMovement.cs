using UnityEngine;

/// <summary>
/// Script to manage the player movement
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    // Variable to specify the time until the movement speed reaches its maximum
    public float speedDampTime = 0.1f;

    // Booleans to check if the player is walking or jumping
    [HideInInspector]
    public bool isWalking = false;
    [HideInInspector]
    public bool isJumping = false;

    [SerializeField]
    private float movForce = 5f;

    // Variables to control the movement and jump
    private float mov;
    
    // groundCheckRadius allows you to set a radius for the groundCheck
    [SerializeField]
    private float groundCheckRadius = 0.15f;

    // This bool is to tell us whether you are on the ground or not
    // the layermask lets you select a layer to be ground
    // You will need to create a layer named Ground and assign your
    // ground objects to this layer.
    private bool isGrounded;
    private bool jump = false;

    private LayerMask groundLayer;

    private Animator anim;

    private Rigidbody rb;
    
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        groundLayer = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
        // Input in Update so there is no problems with the controls
        mov = Input.GetAxis("Horizontal");
        jump = Input.GetButton("Jump");

        if (jump && isGrounded && anim.GetCurrentAnimatorStateInfo(0).fullPathHash == HashIDs.instance.locomotionState)
            isJumping = true;
    }

    private void FixedUpdate()
    {
        MovementManagement(mov, jump);

        // Determines whether isGrounded is true or false by seeing if groundcheck overlaps with some object of the ground layer
        isGrounded = Physics.CheckSphere(transform.position - new Vector3(0f, 0.5f, 0f), groundCheckRadius, groundLayer);

        anim.SetBool(HashIDs.instance.isGroundedBool, isGrounded);
        
        // If it is not grounded then the player can move sideways midair
        if (!isGrounded)
        {
            rb.velocity = new Vector3(mov * movForce, rb.velocity.y, rb.velocity.z);
        }
    }

    private void MovementManagement(float horizontal, bool jump)
    {
        anim.SetBool(HashIDs.instance.jumpBool, jump);
        
        if (horizontal != 0f)
        {
            if (horizontal > 0) GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(0f, 90f, 0f));
            else if(horizontal < 0) GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(0f, -90f, 0f));
            anim.SetFloat(HashIDs.instance.speedFloat, 5.5f, speedDampTime, Time.deltaTime);

            if (anim.GetCurrentAnimatorStateInfo(0).fullPathHash == HashIDs.instance.locomotionState)
                isWalking = true;
            else
                isWalking = false;
        }
        else
        {
            anim.SetFloat(HashIDs.instance.speedFloat, 0f);

            isWalking = false;
        }
    }

}
