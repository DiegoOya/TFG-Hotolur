using UnityEngine;

/// <summary>
/// ScriptableObject to label the variables and functions needed by a potion
/// </summary>
[CreateAssetMenu(fileName = "New Potion", menuName = "Inventory/Item/Potion")]
public class Potion : Item {
    
    // Percentage that this potion will heal the player 
    public int percentageHealthToHeal;

    // The potion will need access to PlayerHealth to add the health cured
    private PlayerHealth playerHealth;

    // Heal the player
    public override void Use()
    {
        base.Use();

        playerHealth = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerHealth>();

        // This will use the object and cure the player healthToHeal % of the player
        playerHealth.HealPlayer(percentageHealthToHeal);
    }

}
