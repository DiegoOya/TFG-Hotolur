using UnityEngine;

/// <summary>
/// Script used to manage the Jump animation of the player
/// </summary>
public class JumpController : StateMachineBehaviour {
    // Force used to jump, the max time of the jump to be allowed to happen,
    // and a counter to track how long it has been jumping
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpTime;
    private float jumpTimeCounter;

    // The stoppedJumping bool lets us track when the player stops jumping.
    private bool stoppedJumping;
    
    private Rigidbody rb;

    private PlayerHealth playerHealth;

    private AudioSource playerAudioSource;

    private void Awake()
    {
        rb = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<Rigidbody>();
        playerHealth = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerHealth>();

        playerAudioSource = GameObject.FindGameObjectWithTag(Tags.player).GetComponents<AudioSource>()[1];
    }

    // Called when the animation starts
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // Set the jumpTimeCounter to whatever we set our jumptime to in the editor
        jumpTimeCounter = jumpTime;

        // Add the velocity in the Y axis so the player can jump and stoppedJumping initialized to false
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        stoppedJumping = false;

        playerAudioSource.Play();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    // Check if the Player is still jumping, if it is, then keep the player on the air
    // and if it is not, then stops the Jump animation
    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {        
        // If the player died then he cannot continue jumping
        float health = playerHealth.GetHealth();
        if (health > 0f)
        {
            // If the Jump button is being kept holding down
            if ((Input.GetButton("Jump")) && !stoppedJumping)
            {
                // And jumpTimeCounter hasn't reached zero
                if (jumpTimeCounter > 0)
                {
                    // Keep jumping and decrease jumpTimeCounter
                    rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                    jumpTimeCounter -= Time.deltaTime;
                }
                else
                {
                    stoppedJumping = true;
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
        else
        {
            animator.SetBool(HashIDs.instance.deadBool, true);
        }
    }
}
