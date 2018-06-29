using UnityEngine;

/// <summary>
/// Script to control the behaviour of the bullets
/// </summary>
public class BulletController : MonoBehaviour {
    
    private Transform player;

    [SerializeField]
    public float shotSpeed = 5f;
    private float maxDamage;
    private float range;
    private float timeUntilShot = 0f;
    private float timeToDeactivate;

    private Vector3 posIni;

    private Rigidbody rb;

    private ObjectPooler objPooler;

    private void Awake()
    {
        // Initialize variables
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        rb = GetComponent<Rigidbody>();
        objPooler = ObjectPooler.instance;
    }

    private void Update()
    {
        // Update timeUntilShot
        timeUntilShot += Time.deltaTime;

        if (timeToDeactivate < timeUntilShot)
        {
            // Deactivate the gameObject
            timeUntilShot = 0f;
            rb.velocity = Vector3.zero;
            gameObject.SetActive(false);
        }
    }

    // This method is called whenever this object spawns 
    public void OnObjectSpawn()
    {
        timeUntilShot = 0f;
        posIni = transform.position;

        rb.velocity = player.forward * shotSpeed;

        timeToDeactivate = range / shotSpeed;
    }

    // Called if the attack touches the enemy
    private void OnTriggerEnter(Collider other)
    {
        // If it touches the player
        if (other.gameObject.CompareTag(Tags.enemy))
        {
            float distSqr = (transform.position.x - posIni.x) * (transform.position.x - posIni.x) +
            (transform.position.y - posIni.y) * (transform.position.y - posIni.y);

            // The damage is maximum as the target is farther
            float damage = (1 - Mathf.Sqrt(distSqr) / range) * maxDamage;

            // The player takes the damage
            EnemyHealth enemy = other.gameObject.GetComponent<EnemyHealth>();
            enemy.TakeDamage(damage);
            Debug.Log("The damage taken by " + other.gameObject.name + " is " + damage);
            
            float enemyHealth = enemy.GetHealth();
            float enemyMaxHealth = enemy.maxHealth;
            GameObject hitParticles = objPooler.SpawnFromPool("Shot Received", transform.position, Quaternion.Euler(Vector3.back));
            ChangeColorParticles(hitParticles, enemyHealth, enemyMaxHealth);

            // Deactivate the gameObject
            timeUntilShot = 0f;
            rb.velocity = Vector3.zero;
            gameObject.SetActive(false);
        }
    }

    // Change the color of the particles depending of the enemy health
    private void ChangeColorParticles(GameObject shot, float enemyHealth, float enemyMaxHealth)
    {
        ParticleSystem colorParticleShot = shot.GetComponent<ParticleSystem>();
        // If the enemy health is inside the range (50%, 100%] then the color of the shot is green
        if (enemyHealth <= enemyMaxHealth && enemyHealth > 0.5f * enemyMaxHealth)
        {
            colorParticleShot.startColor = Color.green;
        }
        else
        {
            // If the enemy health is less than the 50% then the color of the shot is red
            if (enemyHealth <= 0.5f * enemyMaxHealth)
            {
                colorParticleShot.startColor = Color.red;
            }
        }
    }

    // Set the maxDamage to mdmg
    public void SetMaxDamage(float mdmg)
    {
        maxDamage = mdmg;
    }

    // Set the range to r
    public void SetRange(float r)
    {
        range = r;
    }

}
