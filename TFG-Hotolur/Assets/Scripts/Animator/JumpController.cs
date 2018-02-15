using UnityEngine;

public class JumpController : StateMachineBehaviour {
    /*these floats are the force you use to jump, the max time you want your jump to be allowed to happen,
     * and a counter to track how long you have been jumping*/
    public float jumpForce;
    public float movForce;
    public float jumpTime;

    /*the public transform is how you will detect whether we are touching the ground.
     * Add an empty game object as a child of your player and position it at your feet, where you touch the ground.
     * the float groundCheckRadius allows you to set a radius for the groundCheck, to adjust the way you interact with the ground*/
    public float groundCheckRadius;
    private Transform player;

    /*this bool is to tell us whether you are on the ground or not
     * the layermask lets you select a layer to be ground; you will need to create a layer named ground(or whatever you like) and assign your
     * ground objects to this layer.
     * The stoppedJumping bool lets us track when the player stops jumping.*/
    private bool isGrounded;
    private LayerMask groundLayer;
    private bool stoppedJumping;
    
    private Rigidbody rb;
    private Animator anim;

    private float jumpTimeCounter;
    private HashIDs hash;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        rb = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<Rigidbody>();
        anim = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<Animator>();

        groundLayer = LayerMask.GetMask("Ground");

        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        //sets the jumpCounter to whatever we set our jumptime to in the editor
        jumpTimeCounter = jumpTime;

        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        stoppedJumping = false;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        float h = Input.GetAxis("Horizontal");
        rb.velocity = new Vector3(h * movForce, rb.velocity.y, rb.velocity.z);

        //determines whether our bool, grounded, is true or false by seeing if our groundcheck overlaps something on the ground layer
        isGrounded = Physics.CheckSphere(player.position - new Vector3(0f, 0.5f, 0f), groundCheckRadius, groundLayer);
                
        anim.SetBool(hash.isGroundedBool, isGrounded);
        
        //if you keep holding down the mouse button...
        if ((Input.GetButton("Jump")) && !stoppedJumping)
        {
            //and your counter hasn't reached zero...
            if (jumpTimeCounter > 0)
            {
                //keep jumping!
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                jumpTimeCounter -= Time.deltaTime;
            }
        }

        //if you stop holding down the mouse button...
        if (Input.GetButtonUp("Jump"))
        {
            //stop jumping and set your counter to zero.  The timer will reset once we touch the ground again in the update function.
            jumpTimeCounter = 0;
            stoppedJumping = true;
        }
    }
}
