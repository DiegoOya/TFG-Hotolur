using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {
    public float health = 100f;
    public float deactivateTime = 3f;

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        // Animacion Die de prueba
        gameObject.GetComponentInParent<Animator>().
            SetBool(GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>().deadBool, true);

        StartCoroutine(DeactivateEnemy());
    }

    IEnumerator DeactivateEnemy()
    {
        yield return new WaitForSeconds(0.1f);

        gameObject.GetComponentInParent<Animator>().
            SetBool(GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>().deadBool, false);

        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<CapsuleCollider>().enabled = false;

        yield return new WaitForSeconds(deactivateTime);

        gameObject.SetActive(false);
    }
}
