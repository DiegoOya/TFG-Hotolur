using System.Collections;
using UnityEngine;

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

        // Deactivate the Dead animation, the gravity and the collider of the enemy
        anim.SetBool(HashIDs.instance.deadBool, false);

        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<CapsuleCollider>().enabled = false;

        yield return new WaitForSeconds(deactivateTime);

        // Deactivate the GameObject
        gameObject.SetActive(false);
    }
}
