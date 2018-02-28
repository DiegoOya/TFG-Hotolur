using UnityEngine;

public class SmallEnemyAttack : MonoBehaviour, IEnemyAttack {

    public Transform enemyHand;

    ObjectPooler objPooler;

    private void Start()
    {
        objPooler = ObjectPooler.instance;
    }

    // Here is the AI of the attack and the attack per se
    public void Attack(Transform player, Transform enemy, float range)
    {
        objPooler.SpawnFromPool("Small Weapon", enemyHand.position, Quaternion.identity, player, enemy, range);
    }

}
