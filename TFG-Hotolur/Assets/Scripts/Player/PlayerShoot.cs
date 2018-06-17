using UnityEngine;

/// <summary>
/// Script to manage the attack of the player
/// </summary>
public class PlayerShoot : MonoBehaviour {
    
    // Variables to control the parameters of the gun held by the player
    public float maxDamage = 10f;
    public float range = 50f;
    public float fireRate = 5f;

    // Variables to determine the damage of the gun and the next time the gun shoots
    private float damage = 0f;
    private float timerNextShot = 0f;

    private Transform player;

    private AudioSource audioSource;

    private Animator anim;

    private LineRenderer gunLine;

    private void Awake()
    {
        anim = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        gunLine = GetComponent<LineRenderer>();
        gunLine.enabled = false;

        player = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<Transform>();
    }

    private void Update()
    {
        timerNextShot += Time.deltaTime;
        
        // If Fire1 button is pressed and has passed timeNextShot, then shoot
        if (!GameController.instance.doingSetup && (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2")) && Time.time >= 1f / fireRate &&
            anim.GetCurrentAnimatorStateInfo(1).fullPathHash != HashIDs.instance.hitState &&
            anim.GetCurrentAnimatorStateInfo(1).fullPathHash != HashIDs.instance.pickUpState &&
            anim.GetCurrentAnimatorStateInfo(0).fullPathHash != HashIDs.instance.dyingState)
        {
            // Update timeNextShot and shoot
            timerNextShot = 0f;
            Shoot();
        }

        if (timerNextShot >= 0.2f / fireRate)
        {
            // Stop the sound effect
            audioSource.Stop();

            gunLine.enabled = false;
        }
    }

    private void Shoot()
    {
        // If audioSource is not playing then play the sound effect 
        if(!audioSource.isPlaying)
            audioSource.Play();
        
        // Enable gunLine and set the initial position
        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);

        // Apply a raycast and if it hit something, make sure it is the enemy and deal damage
        RaycastHit hit;
        if (Physics.Raycast(transform.position, player.forward, out hit, range))
        {
            if (hit.transform.CompareTag(Tags.enemy))
            {
                //Calculates damage proportional by the distance
                damage = CalculateDamage(hit);
                Debug.Log("Enemy attacked: " + damage + " damage");
                hit.transform.GetComponent<EnemyHealth>().TakeDamage(damage);
            }
            if (!hit.transform.CompareTag(Tags.checkpoint))
                gunLine.SetPosition(1, hit.point);
        }
        else
        {
            gunLine.SetPosition(1, transform.position + player.forward * range);
        }
    }
    
    // Calculates the damage depending on the distance from the player to the enemy
    private float CalculateDamage(RaycastHit hit)
    {
        float distSqr = (hit.point.x - transform.position.x) * (hit.point.x - transform.position.x) +
            (hit.point.y - transform.position.y) * (hit.point.y - transform.position.y);
        float dmg = (1 - Mathf.Sqrt(distSqr) / range) * maxDamage;
        
        return dmg;
    }

}
