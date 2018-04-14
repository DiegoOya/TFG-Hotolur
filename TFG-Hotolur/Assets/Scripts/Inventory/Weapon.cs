using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Weapon")]
public class Weapon : Item {

    public float maxDamage;
    public float fireRate;
    public float range;

    private PlayerShoot playerShoot;

    public override void Use()
    {
        base.Use();

        playerShoot = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerShoot>();

        // This will equip the weapon and return the weapon the player has equipped
        // If there is a model of the gun replace it with the actual weapon of the player
        playerShoot.maxDamage = maxDamage;
        playerShoot.fireRate = fireRate;
        playerShoot.range = range;
    }

}
