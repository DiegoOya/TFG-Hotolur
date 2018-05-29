using UnityEngine;

/// <summary>
/// ScriptableObject to label the variables and functions needed by a weapon
/// </summary>
[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Weapon")]
public class Weapon : Item {

    // Max damage, fire rate and range that will be updated in PlayerShoot
    public float maxDamage;
    public float fireRate;
    public float range;

    // The weapon will need access to PlayerShoot to change the parameters
    private PlayerShoot playerShoot;

    // Equip the weapon
    public override void Use()
    {
        base.Use();

        // Initialize playerShoot
        playerShoot = GameObject.FindGameObjectWithTag(Tags.player).GetComponentInChildren<PlayerShoot>();

        // This will equip the weapon and return the weapon the player has equipped
        // If there is a model of the gun replace it with the actual weapon of the player
        playerShoot.maxDamage = maxDamage;
        playerShoot.fireRate = fireRate;
        playerShoot.range = range;
    }

}
