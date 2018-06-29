using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Script used to manage the health of the enemies and when the enemy dies
/// </summary>
public class EnemyHealth : MonoBehaviour {

    [System.Serializable]
    public class DropItems
    {
        public string nameItem;
        public float probabilityDrop;
    }

    // Number of points the player gets when this enemy dies
    public float points = 10;

    // Maximum health of the character and the time the enemy lasts to deactivate
    public float maxHealth = 100f;
    public float deactivateTime = 3f;

    private float health;
    [SerializeField]
    private float probabilityDropItem = 0.5f;
    [SerializeField]
    private float factorPoints = 10f;

    private bool enemyDead = false;

    [SerializeField]
    private List<DropItems> probabilityItemsList;

    private Animator anim;

    private Enemy enemy;

    private ObjectPooler objPooler;

    private HeadController headController;

    private void Start()
    {
        // Initialize health
        health = maxHealth;

        anim = gameObject.GetComponent<Animator>();

        enemy = GetComponent<Enemy>();

        objPooler = ObjectPooler.instance;

        headController = GameObject.FindGameObjectWithTag(Tags.head).GetComponent<HeadController>();
    }

    // Make the enemy lose health
    public void TakeDamage(float damage)
    {
        // If the player is not detected by the enemy yet then the enemy starts to follow the player 
        if(!enemy.GetPlayerDetected())
        {
            enemy.Interact();
        }

        health -= damage;

        // If the enemy reaches health to 0 the enemy dies
        if (health <= 0f && !enemyDead)
        {
            Die();
            enemyDead = true;
        }
    }

    // Called when the enemy dies
    private void Die()
    {
        anim.SetBool(HashIDs.instance.deadBool, true);

        // Multiply the points given by the enemy and scaled it by a factor determined by the time between the player and head
        float factor = headController.time / factorPoints;
        factor = factor <= 0.5f ? 0.5f : factor;
        points = points * factor;
        GameController.instance.UpdatePoints(Mathf.FloorToInt(points));

        StartCoroutine(DeactivateEnemy());
    }

    // Coroutine to manage the Dead animation
    private IEnumerator DeactivateEnemy()
    {
        yield return new WaitForSeconds(0.1f);

        // See if the enemy is going to drop an item and what item
        DropItem();

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

    // With a probability the enemy drops an item
    private void DropItem()
    {
        float drop = Random.value;
        if (drop <= probabilityDropItem)
        {
            float whatItem = Random.value;
            float item = 0f;
            for(int i = 0; i < probabilityItemsList.Count; i++)
            {
                if(whatItem >= item && whatItem <= (item + probabilityItemsList[i].probabilityDrop))
                {
                    objPooler.SpawnFromPool(probabilityItemsList[i].nameItem, 
                                            new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), 
                                            Quaternion.LookRotation(Camera.main.transform.position));
                }

                item += probabilityItemsList[i].probabilityDrop;
            }
        }
    }

    public float GetHealth()
    {
        return health;
    }

}
