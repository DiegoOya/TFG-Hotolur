using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script to manage the player health and the death of the player
/// </summary>
public class PlayerHealth : MonoBehaviour {

    // Health variables, maxHealth can be modified at the editor
    public float maxHealth = 100f;

    private float health = 100;

    // Slider that will tell the player health
    private Slider sliderHealth;

    private Animator anim;

    // Initialize variables
    private void Awake()
    {
        health = maxHealth;

        sliderHealth = GameObject.FindGameObjectWithTag(Tags.healthBar).GetComponent<Slider>();

        health = maxHealth;

        // Adjust the size and value of the slider
        SliderSize();
        SliderValue();

        anim = GetComponent<Animator>();
    }


    //*****Update de prueba para comprobar que baja la vida de verdad
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            TakeDamage(10f);
    }


    // Called when the enemy deals damage to the player
    public void TakeDamage(float damage)
    {
        // Decrease health and show the result at the slider
        health -= damage;

        SliderValue();

        // If the player health is less than 0, then the player dies
        if (health <= 0f)
        {
            Die();
        }
    }

    // Called when the player uses a potion
    // Heal the player and show the result at the slider
    public void HealPlayer(float percentageHealth)
    {
        health += maxHealth * percentageHealth / 100;
        health = Mathf.Min(health, maxHealth);

        SliderValue();
    }

    // Adjust the slider size
    void SliderSize()
    {
        RectTransform rectBar = sliderHealth.gameObject.GetComponent<RectTransform>();
        rectBar.sizeDelta = new Vector2(maxHealth, rectBar.sizeDelta.y);
        sliderHealth.maxValue = maxHealth;
    }

    // Modify the slider value
    private void SliderValue()
    {
        sliderHealth.value = health;
    }

    // The player dies and the Dead animation is called
    private void Die()
    {
        // Animacion Die de prueba
        anim.SetBool(HashIDs.instance.deadBool, true);
        
        StartCoroutine(DeactivateCollider());
    }

    // When the player dies deactivate the colliders of the GameObject so it doesn't
    // collide with anything
    private IEnumerator DeactivateCollider()
    {
        yield return new WaitForSeconds(0.1f);
        
        anim.SetBool(HashIDs.instance.deadBool, false);

        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<CapsuleCollider>().enabled = false;
    }

    // Called to set maxHealth from other scripts 
    public void SetMaxHealth(float maxHP)
    {
        maxHealth = maxHP;
        SliderSize();
    }

}
