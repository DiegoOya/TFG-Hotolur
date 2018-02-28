using UnityEngine;

public interface IEnemyAttack {

    // Attack the player
    void Attack(Transform player, Transform enemy, float range);

}
