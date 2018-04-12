using UnityEngine;

[CreateAssetMenu(fileName = "New Time Item", menuName = "Inventory/Item/Potion")]
public class TimeItem : Item {

    public int timeExtra;

    public override void Use()
    {
        base.Use();

        // This will use the object and give the player the value of timeExtra of time
        
        // Get the time
        // Add the time timeExtra seconds
    }

}
