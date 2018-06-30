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

    public GameObject pistol;
    public GameObject shotgun;
    public GameObject rifle;

    public Transform endGunPistol;
    public Transform endGunShotgun;
    public Transform endGunRifle;

    public MeshRenderer rendPistol;
    public MeshRenderer rendShotgun;
    public MeshRenderer rendRifle;

    // Number of bullets the enemy is going to have until the weapon overloads
    [SerializeField]
    private int shotgunBulletsToOverload = 8;
    [SerializeField]
    private int rifleBulletsToOverload = 30;
    private int shotgunBulletsUsed = 0;
    private int rifleBulletsUsed = 0;


    // Variables to determine when the next shot is going to be and the cool down of the weapons
    private float timerNextShot = 0f;
    [SerializeField]
    private float timeCoolDown = 5f;
    private float shotgunCoolDown = 0f;
    private float rifleCoolDown = 0f;
    [SerializeField]
    private float volumeSounds = 0.7f;

    private GameObject actualWeapon;

    private Transform player;

    private Animator anim;

    private Color colorPistol;
    private Color colorShotgun;
    private Color colorRifle;
    
    //private MeshRenderer renderer;

    private ObjectPooler objPooler;

    private void Start()
    {
        anim = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<Transform>();
        
        colorPistol = matPistol.color;
        colorShotgun = matShotgun.color;
        colorRifle = matRifle.color;

        //renderer = transform.parent.GetComponentInChildren<MeshRenderer>();

        actualWeapon = pistol;

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

        if(weaponType == WeaponTypes.Pistol)
        {
            //if(renderer == null)
            //{
            //    renderer = transform.parent.GetComponentInChildren<MeshRenderer>();
            //}
            if (actualWeapon != pistol)
            {
                actualWeapon.SetActive(false);
                pistol.SetActive(true);
                actualWeapon = pistol;
            }
            rendPistol.material.color = colorPistol;
        }
        else
        {
            //if (renderer == null)
            //{
            //    renderer = transform.parent.GetComponentInChildren<MeshRenderer>();
            //}
            if (weaponType == WeaponTypes.Shotgun)
            {
                if(actualWeapon != shotgun)
                {
                    actualWeapon.SetActive(false);
                    shotgun.SetActive(true);
                    actualWeapon = shotgun;
                }
                if (shotgunBulletsUsed >= shotgunBulletsToOverload)
                    rendShotgun.material.color = Color.red;
                else
                    rendShotgun.material.color = colorShotgun;
            }
            else
            {
                //if (renderer == null)
                //{
                //    renderer = transform.parent.GetComponentInChildren<MeshRenderer>();
                //}
                if (actualWeapon != rifle)
                {
                    actualWeapon.SetActive(false);
                    rifle.SetActive(true);
                    actualWeapon = rifle;
                }
                if (rifleBulletsUsed >= rifleBulletsToOverload)
                    rendRifle.material.color = Color.red;
                else
                    rendRifle.material.color = colorRifle;
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

        Vector3 pos;

        switch (weaponType)
        {
            case WeaponTypes.Pistol:
                // Play the shot sound
                AudioSource.PlayClipAtPoint(audioPistol, transform.position, volumeSounds + 0.2f);

                // Spawn a bullet in the pool, assign its values and initialize it
                pos = new Vector3(endGunPistol.position.x, endGunPistol.position.y, 0f);
                shot = objPooler.SpawnFromPool("Bullet", pos, Quaternion.Euler(player.transform.forward));
                bulletController = shot.GetComponent<BulletController>();
                bulletController.SetMaxDamage(maxDamage);
                bulletController.SetRange(range);
                shot.GetComponent<BulletController>().OnObjectSpawn();

                break;

            case WeaponTypes.Shotgun:
                if (shotgunBulletsUsed < shotgunBulletsToOverload)
                {
                    // Play the shot sound
                    AudioSource.PlayClipAtPoint(audioShotgun, transform.position, volumeSounds);

                    // Spawn a bullet in the pool, assign its values and initialize it
                    pos = new Vector3(endGunShotgun.position.x, endGunShotgun.position.y, 0f);
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
                    // Play the shot sound
                    AudioSource.PlayClipAtPoint(audioRifle, transform.position, volumeSounds - 0.1f);

                    // Spawn a bullet in the pool, assign its values and initialize it
                    pos = new Vector3(endGunRifle.position.x, endGunRifle.position.y, 0f);
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
