﻿using UnityEngine;

/// <summary>
/// Interface to simplify the attack of different type of enemies
/// </summary>
public interface IEnemyAttack {

    // Used to attack the player
    void Attack(Transform player, Transform enemy, float range);

}
