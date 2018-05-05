using System.Collections;
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
    private float timeNextShot = 0f;

    private LineRenderer laserShot;

    private Animator anim;

    private PlayerMovement playerMovement;

    // Initialize laserShot
    private void Awake()
    {
        laserShot = GetComponent<LineRenderer>();
        anim = GetComponentInParent<Animator>();
        playerMovement = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        // If Fire1 button is pressed and has passed timeNextShot, then shoot
        if (Input.GetButton("Fire1") && Time.time >= timeNextShot &&
            anim.GetNextAnimatorStateInfo(1).fullPathHash != HashIDs.instance.hitState &&
            anim.GetNextAnimatorStateInfo(1).fullPathHash != HashIDs.instance.pickUpState)
        {
            // Update timeNextShot and shoot
            timeNextShot = Time.time + 1f / fireRate;
            Shoot();
        }

        // If Fire1 is unpressed, then set shoot in the animator to false
        if(Input.GetButtonUp("Fire1"))
            anim.SetBool(HashIDs.instance.shootBool, false);
    }

    private void Shoot()
    {
        //*********No me gusta el LineRenderer, pero como forma de Debug no esta mal 
        StartCoroutine(ShotEffect());

        anim.SetBool(HashIDs.instance.shootBool, true);

        laserShot.SetPosition(0, transform.position);

        // Apply a raycast and if it hit something, make sure it is the enemy and deal damage
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            if (hit.transform.CompareTag(Tags.enemy))
            {
                laserShot.SetPosition(1, hit.point);

                //Calculates damage proportional by the distance
                damage = CalculateDamage(hit);

                hit.transform.GetComponent<EnemyHealth>().TakeDamage(damage);
            }
        }
        else
        {
            laserShot.SetPosition(1, transform.forward * range);
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

    // Called to control the time the line renderer is active
    private IEnumerator ShotEffect()
    {
        laserShot.enabled = true;

        yield return new WaitForSeconds(0.07f);

        laserShot.enabled = false;
    }
}
