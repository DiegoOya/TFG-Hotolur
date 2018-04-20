using UnityEngine;

/// <summary>
/// Script used to manage the Jump animation of the player
/// </summary>
public class JumpController : StateMachineBehaviour {
    // Force used to jump, the max time of the jump to be allowed to happen,
    // and a counter to track how long it has been jumping
    public float jumpForce;
    public float movForce;
    public float jumpTime;

    // The public transform is how you will detect whether we are touching the ground.
    // Add an empty game object as a child of your player and position it at your feet, where you touch the ground.
    // the float groundCheckRadius allows you to set a radius for the groundCheck, to adjust the way you interact with the ground
    public float groundCheckRadius;

    private Transform player;

    // This bool is to tell us whether you are on the ground or not
    // the layermask lets you select a layer to be ground
    // You will need to create a layer named ground(or whatever you like) and assign your
    // ground objects to this layer.
    // The stoppedJumping bool lets us track when the player stops jumping.
    private bool isGrounded;
    private bool stoppedJumping;

    private LayerMask groundLayer;
    
    private Rigidbody rb;

    private Animator anim;

    private float jumpTimeCounter;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        rb = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<Rigidbody>();
        anim = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<Animator>();

        groundLayer = LayerMask.GetMask("Ground");
    }

    // Called when the animation starts
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // Set the jumpTimeCounter to whatever we set our jumptime to in the editor
        jumpTimeCounter = jumpTime;

        // Add the velocity in the Y axis so the player can jump and stoppedJumping initialized to false
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        stoppedJumping = false;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    // Check if the Player is still jumping, if it is, then keep the player on the air
    // and if it is not, then stops the Jump animation
    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Get the horizontal input so the player could move sideways in midair
        float h = Input.GetAxis("Horizontal");
        rb.velocity = new Vector3(h * movForce, rb.velocity.y, rb.velocity.z);

        // Determines whether isGrounded is true or false by seeing if groundcheck overlaps with some object of the ground layer
        isGrounded = Physics.CheckSphere(player.position - new Vector3(0f, 0.5f, 0f), groundCheckRadius, groundLayer);
                
        anim.SetBool(HashIDs.instance.isGroundedBool, isGrounded);
        
        // If the Jump button is kept holding down
        if ((Input.GetButton("Jump")) && !stoppedJumping)
        {
            // And jumpTimeCounter hasn't reached zero
            if (jumpTimeCounter > 0)
            {
                // Keep jumping and decrease jumpTimeCounter
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                jumpTimeCounter -= Time.deltaTime;
            }
        }

        // If the Jump Button has stopped being holding down
        if (Input.GetButtonUp("Jump"))
        {
            // Stop jumping and set your counter to zero
            // The timer will reset once we touch the ground again in the update function.
            jumpTimeCounter = 0;
            stoppedJumping = true;
        }
    }
}
