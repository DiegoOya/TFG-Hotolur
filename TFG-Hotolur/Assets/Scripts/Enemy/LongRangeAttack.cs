using UnityEngine;
using System.Collections;

/// <summary>
/// Script used to manage the object used as the weapon of the enemy when they trow it
/// This script has to be attached to such weapon 
/// </summary>
public class LongRangeAttack : MonoBehaviour, IPooledObject {

    public float maxDamage = 20f;
    public float timeToTarget = 5f;

    private Transform player;

    [SerializeField]
    private float probStraightThrow = 0.5f;
    private float range;
    private float distance;

    private Rigidbody rb;

    private void Awake()
    {
        // Initialize variables
        rb = GetComponent<Rigidbody>();
    }

    // This method is called whenever this object spawns 
    public void OnObjectSpawn()
    {
        // Calculate the distance between the attack and the player, the attack will do more damage as the player is farther
        distance = (player.position.x - transform.position.x) * (player.position.x - transform.position.x);

        Vector3 vel = CalculateVelocity();
                
        rb.velocity = vel;

        StartCoroutine(DeactivateWeapon());
    }

    // Called to calculate the parabolic velocity of the attack aiming to the player
    private Vector3 CalculateVelocity()
    {
        // Calculate the distance vector
        Vector3 dist = player.position + new Vector3(0f, 1.2f, 0f) - transform.position;
        float distMagnitude = Mathf.Sqrt(dist.x * dist.x + dist.y * dist.y);

        // Time to reach the player, proportional to the distance between the attack and the player divided by the range 
        float t = (distMagnitude / range) * timeToTarget;
        
        float rectAttack = Random.value;
        if(Mathf.Abs(dist.y) > 3f)
        {
            rectAttack = probStraightThrow;
        }

        // Calculate the velocity of x, y and z axis
        float v0y = 0f;
        float v0x = 0f;
        float v0z = 0f;
        if (rectAttack >= probStraightThrow)
        {
            t = t * 0.8f;
            v0y = dist.y / t;
            v0x = dist.x / t;
            v0z = dist.z / t;
            rb.useGravity = false;
        }
        else
        {
            v0y = (dist.y + 1f) / t + 0.5f * Physics.gravity.magnitude * t;
            v0x = dist.x / t;
            v0z = dist.z / t;
        }

        return new Vector3(v0x, v0y, v0z);
    }

    // Deactivate the weapon if it didn't touch the target
    private IEnumerator DeactivateWeapon()
    {
        yield return new WaitForSeconds(timeToTarget + timeToTarget / 4f);
        
        // Deactivate the gameObject
        IsCollision(true);
        gameObject.SetActive(false);
    }

    // Change isTrigger depending on value, the velocity to 0 and the gravity to value
    private void IsCollision(bool value)
    {
        GetComponent<Collider>().isTrigger = !value;
        rb.velocity = Vector3.zero;
        rb.useGravity = value;
    }

    // Called if the attack touches the player or the ground
    private void OnCollisionEnter(Collision other)
    {
        // If it touches the player
        if (other.gameObject.CompareTag(Tags.player))
        {
            // The damage is maximum as the target is farther
            float damage = (Mathf.Sqrt(distance) / range) * maxDamage;

            // The player takes the damage
            player.GetComponent<PlayerHealth>().TakeDamage(damage);
            Debug.Log("The damage taken by " + other.gameObject.name + " is " + damage);

            gameObject.SetActive(false);
        }
    }

    // Set the player to pj and its animator
    public void SetPlayer(Transform pj)
    {
        player = pj;
    }

    // Set the range to r
    public void SetRange(float r)
    {
        range = r;
    }

}
