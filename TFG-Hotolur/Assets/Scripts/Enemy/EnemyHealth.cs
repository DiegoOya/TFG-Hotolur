using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Script used to manage the health of the enemies and when the enemy dies
/// </summary>
public class EnemyHealth : MonoBehaviour {

    public float maxHealth = 100f;
    public float deactivateTime = 3f;

    private float health;

    private Animator anim;

    private void Start()
    {
        // Initialize health
        health = maxHealth;

        anim = gameObject.GetComponent<Animator>();
    }

    // Make the enemy lose health
    public void TakeDamage(float damage)
    {
        health -= damage;

        // If the enemy reaches health to 0 the enemy dies
        if (health <= 0f)
        {
            Die();
        }
    }

    // Called when the enemy dies
    private void Die()
    {
        // Animacion Die de prueba
        anim.SetBool(HashIDs.instance.deadBool, true);

        StartCoroutine(DeactivateEnemy());
    }

    // Coroutine to manage the Dead animation
    private IEnumerator DeactivateEnemy()
    {
        yield return new WaitForSeconds(0.1f);
        
        // When the enemy dies get it out of the player's way moving it in the Z axis
        // As the camera is ortographic the effect of getting the enemy closer to the camera is undetectable
        transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
        GetComponent<NavMeshAgent>().isStopped = true;

        // Deactivate the Dead animation, the gravity and the collider of the enemy
        //anim.SetBool(HashIDs.instance.deadBool, false);
        
        yield return new WaitForSeconds(deactivateTime);

        // Deactivate the GameObject
        gameObject.SetActive(false);
    }

    public float GetHealth()
    {
        return health;
    }

}
