using UnityEngine;

// This has to be in GameObject which will be used for the attack
public class SmallAttack : MonoBehaviour, IPooledObject {

    public float maxDamage = 20f;
    public float timeToTarget = 5f;

    Transform player;
    Transform enemy;

    float range;
    float damage;
    float distance;

    // This method is called whenever this object spawns 
    public void OnObjectSpawn()
    {
        // Calculates the distance between the attack and the player and the attack will do more damage as the player is farther
        distance = Mathf.Sqrt((player.position.x - transform.position.x) * (player.position.x - transform.position.x));
        damage = (Mathf.Sqrt(distance) / range) * maxDamage; // Use this when this object collides with the player
        
        Vector3 vel = CalculateVelocity();
                
        GetComponent<Rigidbody>().velocity = vel;
    }

    Vector3 CalculateVelocity()
    {
        // Calculate distance vectors
        Vector3 dist = player.position - transform.position;

        float t = (Mathf.Abs(dist.x) / range) * timeToTarget;

        // Calculate the velocity of x and y axis
        float v0y = (dist.y + 1.5f) / t + 0.5f * Physics.gravity.magnitude * t;
        float v0x = dist.x / t;

        return new Vector3(v0x, v0y, 0f);
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
