using UnityEngine;

public class EnemyInteraction : Interactable {

    // Detect player, approach and attack
    public override void Interact()
    {
        base.Interact();

        if (Input.GetKeyDown(KeyCode.L))
        {
            // This search for the script that inherit from IEnemyAttack to attack
            IEnemyAttack enemyAttack = GetComponent<IEnemyAttack>();
            enemyAttack.Attack(player, transform, radius);
        }
    }

}
