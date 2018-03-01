using UnityEngine;
using System.Collections;

public class SmallEnemyAttack : MonoBehaviour, IEnemyAttack {

    public Transform enemyHand;

    public AnimationClip throwAnim;

    ObjectPooler objPooler;

    float attackCoolDown = 0f;
    float timeThrowAnim;

    Animator anim;

    private void Start()
    {
        objPooler = ObjectPooler.instance;
        attackCoolDown -= Time.deltaTime;
        anim = GetComponent<Animator>();
        timeThrowAnim = throwAnim.length;
    }

    private void Update()
    {
        attackCoolDown -= Time.deltaTime;
    }

    // Here is the AI of the attack and the attack per se
    public void Attack(Transform player, Transform enemy, float range)
    {
        if(attackCoolDown <= 0f && !anim.GetBool(Animator.StringToHash("Throw Attack")))
        {
            StartCoroutine(DoAttack(player, enemy, range));
            
            
        }
    }

    IEnumerator DoAttack(Transform player, Transform enemy, float range)
    {
        anim.SetBool(Animator.StringToHash("Throw Attack"), true);

        yield return new WaitForSeconds(0.001f);

        yield return new WaitForSeconds(timeThrowAnim / (4f * anim.GetNextAnimatorStateInfo(0).speed));

        objPooler.SpawnFromPool("Small Weapon", enemyHand.position, Quaternion.identity, player, enemy, range);
        anim.SetBool(Animator.StringToHash("Throw Attack"), false);

        attackCoolDown = timeThrowAnim / anim.GetCurrentAnimatorStateInfo(0).speed;
    }

}
