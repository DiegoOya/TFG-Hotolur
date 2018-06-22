using UnityEngine;

/// <summary>
/// Script to manage the attack of the player
/// </summary>
public class PlayerShoot : MonoBehaviour {
    
    // Different types of weapons
    public enum WeaponTypes {Pistol, Shotgun, Rifle};

    // Variables to control the parameters of the gun held by the player
    public float maxDamage = 10f;
    public float range = 50f;
    public float fireRate = 5f;

    // Type of the weapon equipped, at first is a pistol
    public WeaponTypes weaponType = WeaponTypes.Pistol;

    //[HideInInspector]
    public AudioClip audioPistol;
    //[HideInInspector]
    public AudioClip audioShotgun;
    //[HideInInspector]
    public AudioClip audioRifle;

    // Number of bullets the enemy is going to have until the weapon overloads
    [SerializeField]
    private int shotgunBulletsToOverload = 8;
    [SerializeField]
    private int rifleBulletsToOverload = 30;
    private int shotgunBulletsUsed = 0;
    private int rifleBulletsUsed = 0;


    // Variables to determine the damage of the gun and the next time the gun shoots
    private float damage = 0f;
    private float timerNextShot = 0f;
    [SerializeField]
    private float timeCoolDown = 5f;
    private float shotgunCoolDown = 0f;
    private float rifleCoolDown = 0f;

    private Transform player;

    private AudioSource audioSource;

    private Animator anim;

    private LineRenderer gunLine;

    private ObjectPooler objPooler;

    private void Start()
    {
        anim = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        gunLine = GetComponent<LineRenderer>();
        gunLine.enabled = false;

        player = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<Transform>();

        objPooler = ObjectPooler.instance;
    }

    private void Update()
    {
        timerNextShot += Time.deltaTime;
        
        // If Fire1 button is pressed and has passed timeNextShot, then shoot
        if (!GameController.instance.doingSetup && (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2")) && Time.time >= 1f / fireRate &&
            anim.GetCurrentAnimatorStateInfo(1).fullPathHash != HashIDs.instance.hitState &&
            anim.GetCurrentAnimatorStateInfo(1).fullPathHash != HashIDs.instance.pickUpState &&
            anim.GetCurrentAnimatorStateInfo(0).fullPathHash != HashIDs.instance.dyingState)
        {
            // Update timeNextShot and shoot
            timerNextShot = 0f;
            Shoot();
        }

        if (timerNextShot >= 0.2f / fireRate)
        {
            // Stop the sound effect
            audioSource.Stop();

            gunLine.enabled = false;
        }
    }

    private void Shoot()
    {
        // Used when the weapon is not a pistol
        GameObject shot;
        BulletController bulletController;

        switch (weaponType)
        {
            case WeaponTypes.Pistol:
                // If audioSource is not playing then play the sound effect 
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = audioPistol;
                    audioSource.Play();
                }

                // Enable gunLine and set the initial position
                gunLine.enabled = true;
                gunLine.SetPosition(0, transform.position);

                // Apply a raycast and if it hit something, make sure it is the enemy and deal damage
                RaycastHit hit;
                if (Physics.Raycast(transform.position, player.forward, out hit, range))
                {
                    if (hit.transform.CompareTag(Tags.enemy))
                    {
                        //Calculates damage proportional by the distance
                        damage = CalculateDamage(hit);
                        Debug.Log("Enemy attacked: " + damage + " damage");
                        hit.transform.GetComponent<EnemyHealth>().TakeDamage(damage);

                        // Get the enemy health and apply some particles in the point where the enemy was hit
                        EnemyHealth enemy = hit.transform.gameObject.GetComponent<EnemyHealth>();
                        float enemyHealth = enemy.GetHealth();
                        float enemyMaxHealth = enemy.maxHealth;
                        Vector3 normalDir = hit.normal;
                        GameObject hitParticles = objPooler.SpawnFromPool("Shot Received", hit.point, Quaternion.Euler(normalDir));
                        ChangeColorParticles(hitParticles, enemyHealth, enemyMaxHealth);
                    }
                    if (!hit.transform.CompareTag(Tags.checkpoint))
                        gunLine.SetPosition(1, hit.point);
                }
                else
                {
                    gunLine.SetPosition(1, transform.position + player.forward * range);
                }

                break;

            case WeaponTypes.Shotgun:
                // If audioSource is not playing then play the sound effect 
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = audioShotgun;
                    audioSource.Play();
                }

                // Spawn a bullet in the pool, assign its values and initialize it
                shot = objPooler.SpawnFromPool("Bullet", transform.position, Quaternion.Euler(player.transform.forward));
                bulletController = shot.GetComponent<BulletController>();
                bulletController.SetMaxDamage(maxDamage);
                bulletController.SetRange(range);
                shot.GetComponent<BulletController>().OnObjectSpawn();

                break;

            case WeaponTypes.Rifle:
                // If audioSource is not playing then play the sound effect 
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = audioRifle;
                    audioSource.Play();
                }

                // Spawn a bullet in the pool, assign its values and initialize it
                shot = objPooler.SpawnFromPool("Bullet", transform.position, Quaternion.Euler(player.transform.forward));
                bulletController = shot.GetComponent<BulletController>();
                bulletController.SetMaxDamage(maxDamage);
                bulletController.SetRange(range);
                shot.GetComponent<BulletController>().OnObjectSpawn();

                break;
        }
    }
    
    // Calculates the damage depending on the distance from the player to the enemy
    private float CalculateDamage(RaycastHit hit)
    {
        float distSqr = (hit.point.x - transform.position.x) * (hit.point.x - transform.position.x) +
            (hit.point.y - transform.position.y) * (hit.point.y - transform.position.y);
        float dmg = (1 - Mathf.Sqrt(distSqr) / range) * maxDamage;
        
        return dmg;
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
            // If the enemy health is inside the range (0%, 50%] then the color of the shot is red
            if (enemyHealth <= 0.5f * enemyMaxHealth && enemyHealth > 0)
            {
                colorParticleShot.startColor = Color.red;
            }
            else
            {
                // If the enemy health is zero then the color of the shot is transparent
                if (enemyHealth <= 0)
                {
                    colorParticleShot.startColor = new Color(0, 0, 0, 0);
                }
            }
        }
    }

    // Equip another weapon
    public void EquipWeapon(float maxDmg, float rang, float frRate, WeaponTypes weaponT)
    {
        weaponType = weaponT;
        maxDamage = maxDmg;
        range = rang;
        fireRate = frRate;
        
        if(WeaponTypes.Shotgun == weaponType)
        {
            //shotgunCoolDown = 
        }
    }

}
