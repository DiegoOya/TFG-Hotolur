using UnityEngine;

/// <summary>
/// Script to manage the attack of the player
/// </summary>
public class PlayerShoot : MonoBehaviour {
    
    // Different types of weapons
    public enum WeaponTypes {Pistol, Shotgun, Rifle};

    // Type of the weapon equipped, at first is a pistol
    public WeaponTypes weaponType = WeaponTypes.Pistol;

    // Variables to control the parameters of the gun held by the player
    public float maxDamage = 10f;
    public float range = 50f;
    public float fireRate = 5f;
    
    //[HideInInspector]
    public AudioClip audioPistol;
    //[HideInInspector]
    public AudioClip audioShotgun;
    //[HideInInspector]
    public AudioClip audioRifle;

    //[HideInInspector]
    public Material matPistol;
    //[HideInInspector]
    public Material matShotgun;
    //[HideInInspector]
    public Material matRifle;

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

    private Color colorPistol;
    private Color colorShotgun;
    private Color colorRifle;
    
    private MeshRenderer renderer;

    private ObjectPooler objPooler;

    private void Start()
    {
        anim = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        player = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<Transform>();
        
        colorPistol = matPistol.color;
        colorShotgun = matShotgun.color;
        colorRifle = matRifle.color;

        renderer = transform.parent.GetComponentInChildren<MeshRenderer>();

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
        }

        if(weaponType == WeaponTypes.Pistol)
        {
            if(renderer == null)
            {
                renderer = transform.parent.GetComponentInChildren<MeshRenderer>();
            }
            renderer.material.color = colorPistol;
        }
        else
        {
            if (renderer == null)
            {
                renderer = transform.parent.GetComponentInChildren<MeshRenderer>();
            }
            if (weaponType == WeaponTypes.Shotgun)
            {
                if (shotgunBulletsUsed >= shotgunBulletsToOverload)
                    renderer.material.color = Color.red;
                else
                    renderer.material.color = colorPistol;
            }
            else
            {
                if (renderer == null)
                {
                    renderer = transform.parent.GetComponentInChildren<MeshRenderer>();
                }
                if (rifleBulletsUsed >= rifleBulletsToOverload)
                    renderer.material.color = Color.red;
                else
                    renderer.material.color = colorPistol;
            }
        }

        if (shotgunBulletsUsed >= shotgunBulletsToOverload)
        {
            //matShotgun.color = Color.red;
            shotgunCoolDown += Time.deltaTime;
            if (shotgunCoolDown >= timeCoolDown)
            {
                //matShotgun.color = colorShotgun;
                shotgunBulletsUsed = 0;
                shotgunCoolDown = 0f;
            }
        }

        if (rifleBulletsUsed >= rifleBulletsToOverload)
        {
            //matRifle.color = Color.red;
            rifleCoolDown += Time.deltaTime;
            if (rifleCoolDown >= timeCoolDown)
            {
                //matRifle.color = colorRifle;
                rifleBulletsUsed = 0;
                rifleCoolDown = 0f;
            }
        }
    }

    private void Shoot()
    {
        // Used when the weapon is not a pistol
        GameObject shot;
        BulletController bulletController;

        Vector3 pos = new Vector3(transform.position.x, transform.position.y, 0f);

        switch (weaponType)
        {
            case WeaponTypes.Pistol:
                // If audioSource is not playing then play the sound effect 
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = audioPistol;
                    audioSource.Play();
                }

                // Spawn a bullet in the pool, assign its values and initialize it
                shot = objPooler.SpawnFromPool("Bullet", pos, Quaternion.Euler(player.transform.forward));
                bulletController = shot.GetComponent<BulletController>();
                bulletController.SetMaxDamage(maxDamage);
                bulletController.SetRange(range);
                shot.GetComponent<BulletController>().OnObjectSpawn();

                break;

            case WeaponTypes.Shotgun:
                if (shotgunBulletsUsed < shotgunBulletsToOverload)
                {
                    // If audioSource is not playing then play the sound effect 
                    if (!audioSource.isPlaying)
                    {
                        audioSource.clip = audioShotgun;
                        audioSource.Play();
                    }

                    // Spawn a bullet in the pool, assign its values and initialize it
                    shot = objPooler.SpawnFromPool("Bullet", pos, Quaternion.Euler(player.transform.forward));
                    bulletController = shot.GetComponent<BulletController>();
                    bulletController.SetMaxDamage(maxDamage);
                    bulletController.SetRange(range);
                    shot.GetComponent<BulletController>().OnObjectSpawn();

                    shotgunBulletsUsed++;
                }

                break;

            case WeaponTypes.Rifle:
                if (rifleBulletsUsed < rifleBulletsToOverload)
                {
                    // If audioSource is not playing then play the sound effect 
                    if (!audioSource.isPlaying)
                    {
                        audioSource.clip = audioRifle;
                        audioSource.Play();
                    }

                    // Spawn a bullet in the pool, assign its values and initialize it
                    shot = objPooler.SpawnFromPool("Bullet", pos, Quaternion.Euler(player.transform.forward));
                    bulletController = shot.GetComponent<BulletController>();
                    bulletController.SetMaxDamage(maxDamage);
                    bulletController.SetRange(range);
                    shot.GetComponent<BulletController>().OnObjectSpawn();

                    rifleBulletsUsed++;
                }

                break;
        }
    }

    // Equip another weapon
    public void EquipWeapon(float maxDmg, float rang, float frRate, WeaponTypes weaponT)
    {
        weaponType = weaponT;
        maxDamage = maxDmg;
        range = rang;
        fireRate = frRate;
    }

}
