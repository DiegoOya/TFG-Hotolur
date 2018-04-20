using UnityEngine;

/// <summary>
/// ScriptableObject to label the variables and functions needed by a watch
/// </summary>
[CreateAssetMenu(fileName = "New Time Item", menuName = "Inventory/Item/Potion")]
public class Watch : Item {

    // Time extra that will be added to the time counter
    public int timeExtra;

    // Add time to the time counter
    public override void Use()
    {
        base.Use();

        // This will use the object and give the player the value of timeExtra of time
        
        // Get the time
        // Add the time timeExtra seconds
    }

}
