using UnityEngine;
using System.Collections;

/// <summary>
/// Script used to manage the actions (AI and animations) of the boss enemy when they attack
/// </summary>
public class BossEnemyAttack : MonoBehaviour, IEnemyAttack
{

    // Transform of the enemy hand and the animation of Throw
    // If you want to change the enemyHand or throwAnim, you have to delete [HideInInspector] temporally
    [HideInInspector]
    public Transform enemyHand;
    //[HideInInspector]
    public Transform specialAttack;

    // References of the attack animations
    [HideInInspector]
    public AnimationClip throwAnim;
    [HideInInspector]
    public AnimationClip meleeAnim;
    [HideInInspector]
    public AnimationClip specialAttackAnim;

    private bool isAttacking = false;
    private bool tryingToAttack = false;

    private ObjectPooler objPooler;

    [SerializeField]
    private float shortRangeRadius = 1.5f;
    [SerializeField]
    private float specialAttackSize = 3f;
    [SerializeField]
    private float specialAttackCoolDown = 10f;
    [SerializeField]
    private float specialAttackSmoothing = 5f;

    private float attackCoolDown = 0f;
    private float timeThrowAnim;
    private float timeMeleeAnim;
    private float timeSpecialAttackAnim;

    private Animator anim;

    private Material specialAttackMat;

    // Initialize variables
    private void Start()
    {
        objPooler = ObjectPooler.instance;

        anim = GetComponent<Animator>();
        timeThrowAnim = throwAnim.length;
        timeMeleeAnim = meleeAnim.length;
        timeSpecialAttackAnim = specialAttackAnim.length;

        specialAttack.localScale = new Vector3(specialAttackSize, specialAttack.localScale.y, specialAttack.localScale.z);

        specialAttackMat = specialAttack.GetComponent<Material>();
    }

    private void Update()
    {
        // Decrease attackCoolDown
        attackCoolDown -= Time.deltaTime;

        if (attackCoolDown <= 0f && (!anim.GetBool(HashIDs.instance.throwBool) || 
            !anim.GetBool(HashIDs.instance.meleeBool) || !anim.GetBool(HashIDs.instance.specialAttackBool)))
            isAttacking = false;
    }

    // Here is the AI of the attack and the attack per se
    public void Attack(Transform player, Transform enemy, float range)
    {
        if (attackCoolDown <= 0f && (!anim.GetBool(HashIDs.instance.throwBool) || 
            !anim.GetBool(HashIDs.instance.meleeBool) || !anim.GetBool(HashIDs.instance.specialAttackBool)))
        {
            float probSpecialAttack = 0f;

            if(timeSpecialAttackAnim < 0f)
            {
                probSpecialAttack = Random.value;
            }

            if(probSpecialAttack > 0.98f)
            {
                StartCoroutine(DoSpecialAttack(player));
            }
            else
            {
                // Calculate the distance between the player and the enemy
                float distance = Mathf.Sqrt(
                    (player.position.x - transform.position.x) * (player.position.x - transform.position.x) +
                    (player.position.y - transform.position.y) * (player.position.y - transform.position.y));

                // If the player is inside shortRangeRadius then the attack will be the short range attack
                if (distance < shortRangeRadius)
                {
                    StartCoroutine(DoShortRangeAttack(player));

                    // Reset attackCoolDown
                    attackCoolDown = timeMeleeAnim / anim.GetCurrentAnimatorStateInfo(0).speed;
                }
                else
                {
                    StartCoroutine(DoLongRangeAttack(player, enemy, range));

                    // Reset attackCoolDown
                    attackCoolDown = timeThrowAnim / anim.GetCurrentAnimatorStateInfo(0).speed;
                }
            }
        }
    }

    // Coroutine of the enemy long range attack
    private IEnumerator DoLongRangeAttack(Transform player, Transform enemy, float range)
    {
        // Activate anim
        anim.SetBool(HashIDs.instance.throwBool, true);
        isAttacking = true;

        yield return new WaitForSeconds(0.001f);

        // Wait the time needed to throw the attack in the optimal gesture of the animation
        yield return new WaitForSeconds(timeThrowAnim / (4f * anim.GetNextAnimatorStateInfo(0).speed));

        // Instantiate the attack and deactivate the animation
        objPooler.SpawnFromPool("Small Weapon", enemyHand.position, Quaternion.identity, player, enemy, range);
        anim.SetBool(HashIDs.instance.throwBool, false);
    }

    // Coroutine of the enemy short range attack
    private IEnumerator DoShortRangeAttack(Transform player)
    {
        // Activate anim
        anim.SetBool(HashIDs.instance.meleeBool, true);
        isAttacking = true;

        yield return new WaitForSeconds(0.001f);

        // Wait the time needed to set meleeBool to false
        yield return new WaitForSeconds(timeMeleeAnim / (4f * anim.GetNextAnimatorStateInfo(0).speed));

        anim.SetBool(HashIDs.instance.meleeBool, false);
    }

    // Coroutine of the enemy special attack
    private IEnumerator DoSpecialAttack(Transform player)
    {
        // Activate anim
        anim.SetBool(HashIDs.instance.specialAttackBool, true);
        isAttacking = true;

        yield return new WaitForSeconds(0.001f);

        // Wait the time needed to throw the attack in the optimal gesture of the animation
        yield return new WaitForSeconds(timeSpecialAttackAnim / (4f * anim.GetNextAnimatorStateInfo(0).speed));

        // Activate the special attack and set the alpha component
        specialAttack.gameObject.SetActive(true);
        specialAttackMat.color = new Color(specialAttackMat.color.r, specialAttackMat.color.g, 
            specialAttackMat.color.b, Mathf.Clamp(0f, 50f, specialAttackSmoothing * Time.deltaTime));

        anim.SetBool(HashIDs.instance.specialAttackBool, false);

        yield return new WaitForSeconds(timeSpecialAttackAnim / (2f * anim.GetNextAnimatorStateInfo(0).speed));

        // Deactivate the special attack
        specialAttack.gameObject.SetActive(false);
    }

    // A getter of isAttacking
    public bool GetIsAttacking()
    {
        return isAttacking;
    }

    public bool GetTryingToAttack()
    {
        return tryingToAttack;
    }

}

