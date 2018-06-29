using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

/// <summary>
/// Script used to manage the interaction of the enemy with the player
/// </summary>
public class Enemy : Interactable {

    [SerializeField]
    private float attackRadius = 4f;
    [SerializeField]
    private float turnSmoothing = 15f;

    private Vector3 destination;

    private bool playerDetected = false;

    private IEnemyAttack enemyAttack;

    private Rigidbody rb;

    private PlayerHealth playerHealth;

    private EnemyHealth enemyHealth;

    private NavMeshAgent agent;

    private ThirdPersonCharacter character;

    private void Start()
    {
        // Search for the script that inherit from IEnemyAttack to attack
        enemyAttack = GetComponent<IEnemyAttack>();

        rb = GetComponent<Rigidbody>();
        playerHealth = player.GetComponent<PlayerHealth>();

        enemyHealth = GetComponent<EnemyHealth>();

        // Set the reference of NavMeshAgent
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;

        character = GetComponent<ThirdPersonCharacter>();

        // Set destination so the enemy can move there
    }

    private void FixedUpdate()
    {
        // If the player is detected then the enemy starts to follow the player
        if(playerDetected && !agent.isStopped)
        {
            bool isAttacking = enemyAttack.GetIsAttacking();

            // If the player is outside the stoppingDistance then make the enemy follow the player
            if (distance > attackRadius)
            {
                if (!isAttacking)
                {
                    // Make the eneny follow the player
                    destination = player.position;
                    agent.SetDestination(destination);

                    character.Move(agent.desiredVelocity, false);
                }
            }
            else
            {
                bool tryingToAttack = enemyAttack.GetTryingToAttack();
                if (!isAttacking && !tryingToAttack)
                {
                    agent.SetDestination(transform.position);

                    // When the enemy is inside attackRadius the enemy can attack
                    enemyAttack.Attack(player, transform, attackRadius);
                }

                

                if (tryingToAttack)
                {
                    // Make the eneny follow the player
                    destination = player.position;
                    agent.SetDestination(destination);
                    agent.stoppingDistance = 1.5f;
                    character.Move(agent.desiredVelocity, false);
                }
                else
                {
                    character.Move(Vector3.zero, false);
                }
            }

            // Check if the player is at the enemy's back
            float distEnemyPlayer = transform.position.x - player.position.x;

            // If the player is at its back then make it turn around
            Quaternion newRotation = new Quaternion();
            if (distEnemyPlayer >= 0)
                newRotation = Quaternion.Lerp(rb.rotation, Quaternion.Euler(0f, -90f, 0f), turnSmoothing * Time.deltaTime);
            else if (distEnemyPlayer < 0)
                newRotation = Quaternion.Lerp(rb.rotation, Quaternion.Euler(0f, 90f, 0f), turnSmoothing * Time.deltaTime);

            rb.MoveRotation(newRotation);
        }
    }

    // Detect player, approach and attack
    public override void Interact()
    {
        // If it is the first time the enemy detect the player then apply a random stopping distance the 20% of the attackRadius
        if(!playerDetected)
        {
            agent.stoppingDistance = Random.Range(attackRadius * 0.8f, attackRadius * 1.2f);
            attackRadius = agent.stoppingDistance;

            playerDetected = true;
        }
        else
        {
            if (playerHealth.GetHealth() <= 0 || enemyHealth.GetHealth() <= 0)
            {
                character.Move(Vector3.zero, false);
                agent.isStopped = true;
            }
        }
    }

    // Getter of playerDetected
    public bool GetPlayerDetected()
    {
        return playerDetected;
    }

}
