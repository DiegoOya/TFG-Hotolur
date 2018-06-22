using UnityEngine;

/// <summary>
/// Interface to simplify the attack of different type of enemies
/// </summary>
public interface IEnemyAttack {
    
    // Used to attack the player
    void Attack(Transform player, Transform enemy, float range);

    // Used to get if the enemy is in the middle of an attack animation
    bool GetIsAttacking();
    bool GetTryingToAttack(); // Used to know if the enemy is trying to attack
}
