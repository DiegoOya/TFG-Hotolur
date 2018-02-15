using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {
    public float health = 100;

    private float maxHealth = 100f;
    private Slider sliderHealth;

    private void Awake()
    {
        sliderHealth = GameObject.FindGameObjectWithTag(Tags.healthBar).GetComponent<Slider>();
    }

    private void Start()
    {
        SliderSize();
    }


    //Update de prueba para comprobar que baja la vida de verdad
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            TakeDamage(10f);
    }



    public void TakeDamage(float damage)
    {
        health -= damage;

        SliderValue();

        if (health <= 0f)
        {
            Die();
        }
    }

    void SliderSize()
    {
        RectTransform rectBar = sliderHealth.gameObject.GetComponent<RectTransform>();
        rectBar.sizeDelta = new Vector2(maxHealth, rectBar.sizeDelta.y);
        sliderHealth.maxValue = maxHealth;
    }

    void SliderValue()
    {
        sliderHealth.value = health;
    }

    void Die()
    {
        // Animacion Die de prueba
        gameObject.GetComponentInParent<Animator>().
            SetBool(GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>().deadBool, true);

        StartCoroutine(DeactivateCollider());
    }

    IEnumerator DeactivateCollider()
    {
        yield return new WaitForSeconds(0.1f);

        gameObject.GetComponentInParent<Animator>().
            SetBool(GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>().deadBool, false);

        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<CapsuleCollider>().enabled = false;
    }
}
