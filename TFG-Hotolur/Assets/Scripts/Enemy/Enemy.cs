using UnityEngine;

/// <summary>
/// Script used to manage the interaction of the enemy with the player
/// </summary>
public class Enemy : Interactable {

    // Detect player, approach and attack
    public override void Interact()
    {
        base.Interact();

        // Temporal code to test enemy attack
        if (Input.GetKeyDown(KeyCode.L)) 
        {
            // This search for the script that inherit from IEnemyAttack to attack
            IEnemyAttack enemyAttack = GetComponent<IEnemyAttack>();
            enemyAttack.Attack(player, transform, radius);
        }
    }

}
