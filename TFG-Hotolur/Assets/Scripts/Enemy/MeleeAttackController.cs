using UnityEngine;

/// <summary>
/// Script used to control if the hand of the enemy touches the player
/// and if it does then hurt the player
/// </summary>
public class MeleeAttackController : MonoBehaviour {

    // Radius and damage of the attack
    [SerializeField]
    private float meleeRadius = 0.15f;
    [SerializeField]
    private float meleeDamage = 25f;

    // Reference of the player transform
    private Transform player;

    private PlayerHealth playerHealth;

    private IEnemyAttack enemyAttack;

    // Initialize the variables
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyAttack = GetComponentInParent<IEnemyAttack>();
    }

    private void Update()
    {
        // If the enemy is attacking then calculate if the player is touching the enemy hand
        if(enemyAttack.IsAttacking())
        {
            // Calculate the distance between the player and the enemy hand
            float distance = Mathf.Sqrt(
                (player.position.x - transform.position.x) * (player.position.x - transform.position.x) +
                (player.position.y - transform.position.y) * (player.position.y - transform.position.y));

            // If the player is inside the enemy hand and then hurt the player
            if (distance < meleeRadius)
            {
                playerHealth.TakeDamage(meleeDamage);
            }
        }
    }

}
