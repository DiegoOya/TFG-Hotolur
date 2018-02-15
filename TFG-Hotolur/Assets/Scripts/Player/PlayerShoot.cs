using System.Collections;
using UnityEngine;

public class PlayerShoot : MonoBehaviour {
    public float maxDamage = 10f;
    public float range = 50f;
    public float fireRate = 5f;

    float damage = 0f;
    private float timeNextShot = 0f;
    private LineRenderer laserShot;

    private void Awake()
    {
        laserShot = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= timeNextShot)
        {
            timeNextShot = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        // No me gusta el LineRenderer, pero como forma de Debug no esta mal 
        StartCoroutine(ShotEffect());

        laserShot.SetPosition(0, transform.position);

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            laserShot.SetPosition(1, hit.point);

            //Calculates damage from the distance
            damage = CalculateDamage(hit);

            if (hit.transform.CompareTag(Tags.enemy))
            {
                hit.transform.GetComponent<EnemyHealth>().TakeDamage(damage);
            }
        }
        else
        {
            laserShot.SetPosition(1, transform.forward * range);
        }
    }

    /// <summary>
    /// Calculates the damage depending on the distance from the player to the enemy
    /// </summary>
    /// <param name="hit"></param>
    /// <returns></returns>
    float CalculateDamage(RaycastHit hit)
    {
        float distSqr = (hit.point.x - transform.position.x) * (hit.point.x - transform.position.x) +
            (hit.point.y - transform.position.y) * (hit.point.y - transform.position.y);
        float dmg = (1 - Mathf.Sqrt(distSqr) / range) * maxDamage;
        
        return dmg;
    }

    IEnumerator ShotEffect()
    {
        laserShot.enabled = true;

        yield return new WaitForSeconds(0.07f);

        laserShot.enabled = false;
    }
}
