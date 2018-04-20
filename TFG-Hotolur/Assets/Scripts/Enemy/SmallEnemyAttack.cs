using UnityEngine;
using System.Collections;

/// <summary>
/// Script used to manage the actions (AI and animations) of the small enemy when they attack
/// </summary>
public class SmallEnemyAttack : MonoBehaviour, IEnemyAttack {
    
    // Transform of the enemy hand and the animation of Throw
    // If you want to change the enemyHand or throwAnim, you have to delete [HideInInspector] temporally
    [HideInInspector]
    public Transform enemyHand;
    [HideInInspector]
    public AnimationClip throwAnim;

    ObjectPooler objPooler;

    float attackCoolDown = 0f;
    float timeThrowAnim;

    Animator anim;

    // Initialize variables
    private void Start()
    {
        objPooler = ObjectPooler.instance;
        anim = GetComponent<Animator>();
        timeThrowAnim = throwAnim.length;
    }

    private void Update()
    {
        // Decrease attackCoolDown
        attackCoolDown -= Time.deltaTime;
    }

    // Here is the AI of the attack and the attack per se ******NOT IMPLEMENTED YET
    public void Attack(Transform player, Transform enemy, float range)
    {
        if(attackCoolDown <= 0f && !anim.GetBool(HashIDs.instance.throwBool))
        {
            StartCoroutine(DoAttack(player, enemy, range));
            
            
        }
    }

    // Coroutine of the enemy attack
    IEnumerator DoAttack(Transform player, Transform enemy, float range)
    {
        // Activate anim
        anim.SetBool(HashIDs.instance.throwBool, true);

        yield return new WaitForSeconds(0.001f);

        // Wait the time needed to throw the attack in the optimal gesture of the animation
        yield return new WaitForSeconds(timeThrowAnim / (4f * anim.GetNextAnimatorStateInfo(0).speed));

        // Instantiate the attack and deactivate the animation
        objPooler.SpawnFromPool("Small Weapon", enemyHand.position, Quaternion.identity, player, enemy, range);
        anim.SetBool(Animator.StringToHash("Throw Attack"), false);

        // Reset attackCoolDown
        attackCoolDown = timeThrowAnim / anim.GetCurrentAnimatorStateInfo(0).speed;
    }

}
