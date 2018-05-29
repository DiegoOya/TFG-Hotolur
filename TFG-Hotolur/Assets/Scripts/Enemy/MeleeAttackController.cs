using UnityEngine;

/// <summary>
/// Script used to control if the hand of the enemy touches the player
/// and if it does then hurt the player
/// </summary>
public class MeleeAttackController : MonoBehaviour {

    // Damage of the attack
    [SerializeField]
    private float meleeDamage = 25f;

    // Reference of PlayerHealth
    private PlayerHealth playerHealth;

    private IEnemyAttack enemyAttack;

    // Initialize the variables
    private void Awake()
    {
        playerHealth = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerHealth>();
        enemyAttack = GetComponentInParent<IEnemyAttack>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the enemy is attacking then calculate if the player is touching the enemy hand
        if (enemyAttack.IsAttacking())
        {
            // If other is the player, then hurt the player
            if(other.CompareTag(Tags.player))
            {
                playerHealth.TakeDamage(meleeDamage);
            }
        }
    }

}
