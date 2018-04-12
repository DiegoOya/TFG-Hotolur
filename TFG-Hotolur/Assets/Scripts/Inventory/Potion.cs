using UnityEngine;

[CreateAssetMenu(fileName = "New Potion", menuName = "Inventory/Item/Potion")]
public class Potion : Item {

    public int percentageHealthToHeal;

    public override void Use()
    {
        base.Use();

        // This will use the object and cure the player healthToHeal % of the player
        PlayerHealth.instance.HealPlayer(percentageHealthToHeal);
    }

}
