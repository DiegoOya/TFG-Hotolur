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

    public AudioClip audioThrow;

    private bool isAttacking = false;
    private bool tryingToAttack = false;

    private ObjectPooler objPooler;

    [SerializeField]
    private float volumeSounds = 0.7f;
    private float attackCoolDown = 0f;
    private float timeThrowAnim;

    private Animator anim;

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

        if (attackCoolDown <= 0f && !anim.GetBool(HashIDs.instance.throwBool))
            isAttacking = false;
    }

    // Here is the AI of the attack and the attack per se
    public void Attack(Transform player, Transform enemy, float range)
    {
        if(attackCoolDown <= 0f && !anim.GetBool(HashIDs.instance.throwBool))
        {
            tryingToAttack = true;
            StartCoroutine(DoAttack(player, enemy, range));

            // Reset attackCoolDown
            attackCoolDown = timeThrowAnim / anim.GetCurrentAnimatorStateInfo(0).speed;
        }
    }

    // Coroutine of the enemy attack
    private IEnumerator DoAttack(Transform player, Transform enemy, float range)
    {
        // Activate anim
        anim.SetBool(HashIDs.instance.throwBool, true);
        isAttacking = true;
        tryingToAttack = false;
        
        yield return new WaitForSeconds(0.001f);

        // Wait the time needed to throw the attack in the optimal gesture of the animation
        yield return new WaitForSeconds(timeThrowAnim / (4f * anim.GetNextAnimatorStateInfo(0).speed));

        // Instantiate the attack and deactivate the animation
        objPooler.SpawnFromPool("Small Weapon", enemyHand.position, Quaternion.identity, player, enemy, range);
        anim.SetBool(HashIDs.instance.throwBool, false);
        
        AudioSource.PlayClipAtPoint(audioThrow, transform.position, volumeSounds);
    }

    public bool GetIsAttacking()
    {
        return isAttacking;
    }

    public bool GetTryingToAttack()
    {
        return tryingToAttack;
    }

}
