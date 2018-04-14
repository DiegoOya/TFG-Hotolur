using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speedDampTime = 0.1f;

    private float mov;

    private bool jump = false;
    private bool palmar = false; // To test the Dead animation

    private Animator anim;

    private GameObject endGun;
    
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // Input in Update so there is no problems with the controls
        mov = Input.GetAxis("Horizontal");

        jump = Input.GetButton("Jump");

        // Button aux for the dead animation
        palmar = Input.GetButtonDown("Dead");
        if(palmar)
            anim.SetBool(HashIDs.instance.deadBool, palmar);

        //AudioManagement();
    }

    private void FixedUpdate()
    {
        MovementManagement(mov, jump);
    }

    void MovementManagement(float horizontal, bool jump)
    {
        anim.SetBool(HashIDs.instance.jumpBool, jump);

        if (horizontal != 0f)
        {
            if (horizontal > 0) GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(0f, 90f, 0f));
            else if(horizontal < 0) GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(0f, -90f, 0f));
            anim.SetFloat(HashIDs.instance.speedFloat, 5.5f, speedDampTime, Time.deltaTime);
        }
        else
        {
            anim.SetFloat(HashIDs.instance.speedFloat, 0f);
        }
    }
    /*
    void AudioManagement()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).fullPathHash == hash.locomotionState)
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Play();
            }
        }
        else
        {
            GetComponent<AudioSource>().Stop();
        }
    }*/
}
