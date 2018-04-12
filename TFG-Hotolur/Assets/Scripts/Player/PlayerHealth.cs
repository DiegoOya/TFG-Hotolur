using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    #region Singleton

    public static PlayerHealth instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    public float health = 100;

    private float maxHealth = 100f;

    private Slider sliderHealth;

    Animator anim;

    private void Start()
    {
        sliderHealth = GameObject.FindGameObjectWithTag(Tags.healthBar).GetComponent<Slider>();
        SliderSize();

        anim = GetComponent<Animator>();
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

    public void HealPlayer(float percentageHealth)
    {
        health += maxHealth * percentageHealth / 100;
        health = Mathf.Min(health, maxHealth);

        SliderValue();
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
        anim.SetBool(HashIDs.instance.deadBool, true);
        
        StartCoroutine(DeactivateCollider());
    }

    // When the player dies deactivate the colliders of the GameObject so it doesn't
    // collide with anything
    IEnumerator DeactivateCollider()
    {
        yield return new WaitForSeconds(0.1f);
        
        anim.SetBool(HashIDs.instance.deadBool, false);

        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<CapsuleCollider>().enabled = false;
    }

    void SetMaxHealth(float maxHP)
    {
        maxHealth = maxHP;
        SliderSize();
    }

}
