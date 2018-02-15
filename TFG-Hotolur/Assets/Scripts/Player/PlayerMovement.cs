using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speedDampTime = 0.1f;
    
    private Animator anim;
    private GameObject endGun;
    private HashIDs hash;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");

        bool jump = Input.GetButtonDown("Jump");

        // Button aux for the dead animation
        bool palmar = Input.GetButtonDown("Dead");
        anim.SetBool(hash.deadBool, palmar);

        MovementManagement(h, jump);
    }

    private void Update()
    {
        //AudioManagement();
    }

    void MovementManagement(float horizontal, bool jump)
    {
        anim.SetBool(hash.jumpBool, jump);

        if (horizontal != 0f)
        {
            if (horizontal > 0) GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(0f, 90f, 0f));
            else if(horizontal < 0) GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(0f, -90f, 0f));
            anim.SetFloat(hash.speedFloat, 5.5f, speedDampTime, Time.deltaTime);
        }
        else
        {
            anim.SetFloat(hash.speedFloat, 0f);
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
