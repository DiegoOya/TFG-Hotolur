using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Weapon")]
public class Weapon : Item {

    public float maxDamage;
    public float fireRate;
    public float range;

    public override void Use()
    {
        base.Use();

        // This will equip the weapon and return the weapon the player has equipped
        // If there is a model of the gun replace it with the actual weapon of the player
        PlayerShoot.instance.maxDamage = maxDamage;
        PlayerShoot.instance.fireRate = fireRate;
        PlayerShoot.instance.range = range;
    }

}
