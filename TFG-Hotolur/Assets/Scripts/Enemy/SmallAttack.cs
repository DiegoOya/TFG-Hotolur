using UnityEngine;
using System.Collections;

// This has to be in GameObject which will be used for the attack
public class SmallAttack : MonoBehaviour, IPooledObject {

    public float maxDamage = 20f;
    public float timeToTarget = 5f;

    Transform player;
    Transform enemy;

    float range;
    float distance;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // This method is called whenever this object spawns 
    public void OnObjectSpawn()
    {
        // Calculates the distance between the attack and the player and the attack will do more damage as the player is farther
        distance = (player.position.x - transform.position.x) * (player.position.x - transform.position.x);

        Vector3 vel = CalculateVelocity();
                
        rb.velocity = vel;

        StartCoroutine(DeactivateWeapon());
    }

    Vector3 CalculateVelocity()
    {
        // Calculate distance vector
        Vector3 dist = player.position - transform.position;

        float t = (Mathf.Abs(dist.x) / range) * timeToTarget;

        // Calculate the velocity of x and y axis
        float v0y = (dist.y + 1f) / t + 0.5f * Physics.gravity.magnitude * t;
        float v0x = dist.x / t;
        float v0z = dist.z / t;

        return new Vector3(v0x, v0y, v0z);
    }

    // Deactivate the weapon if it didn't touch the target
    IEnumerator DeactivateWeapon()
    {
        yield return new WaitForSeconds(timeToTarget + timeToTarget / 4f);

        IsCollision(true);
        gameObject.SetActive(false);
    }

    void IsCollision(bool value)
    {
        GetComponent<Collider>().isTrigger = !value;
        rb.velocity = Vector3.zero;
        rb.useGravity = value;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(Tags.player))
        {
            // The damage is maximum as the target is farther
            float damage = (Mathf.Sqrt(distance) / range) * maxDamage;
            PlayerHealth.instance.TakeDamage(damage);
            Debug.Log("The damage taken by " + other.gameObject.name + " is " + damage);

            gameObject.SetActive(false);
        }

        if(other.gameObject.layer == 8)
        {
            IsCollision(false);
        }
    }

    public void SetPlayer(Transform pj)
    {
        player = pj;
    }

    public void SetRange(float r)
    {
        range = r;
    }

    public void SetEnemy(Transform en)
    {
        enemy = en;
    }

}
