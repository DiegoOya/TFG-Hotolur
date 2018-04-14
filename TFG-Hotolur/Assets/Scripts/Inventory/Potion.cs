using UnityEngine;

[CreateAssetMenu(fileName = "New Potion", menuName = "Inventory/Item/Potion")]
public class Potion : Item {

    public int percentageHealthToHeal;

    private PlayerHealth playerHealth;

    public override void Use()
    {
        base.Use();

        playerHealth = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerHealth>();

        // This will use the object and cure the player healthToHeal % of the player
        playerHealth.HealPlayer(percentageHealthToHeal);
    }

}
