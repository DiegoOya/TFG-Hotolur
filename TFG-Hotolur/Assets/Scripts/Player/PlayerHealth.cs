using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Script to manage the player health and the death of the player
/// </summary>
public class PlayerHealth : MonoBehaviour {

    // Health variables, maxHealth can be modified at the editor
    public float maxHealth = 100f;
    
    private float health = 100;

    // Booleans to check if the player took damage or died
    [HideInInspector]
    public bool isHurt = false;
    [HideInInspector]
    public bool isDead = false;

    // Slider that will tell the player health
    private Slider healthSlider;

    private Image fillColor;

    private Animator anim;

    // Initialize variables
    private void Awake()
    {
        health = maxHealth;

        healthSlider = GameObject.FindGameObjectWithTag(Tags.healthBar).GetComponent<Slider>();
        fillColor = healthSlider.gameObject.GetComponentsInChildren<Image>()[1];

        health = maxHealth;

        // Adjust the size and value of the slider
        SliderSize();
        SliderValue();

        anim = GetComponent<Animator>();
    }
    
    private void Update()
    {
        // If healthSlider is inside the range (50%, 100%] then fillColor is green
        if (healthSlider.value <= maxHealth && healthSlider.value > 0.5f * maxHealth)
        {
            fillColor.color = Color.green;
        }
        else
        {
            // If healthSlider is inside the range (25%, 50%] then fillColor is orange
            if (healthSlider.value <= 0.5f * maxHealth && healthSlider.value > 0.25f * maxHealth)
            {
                fillColor.color = new Color(255, 178, 0, 255);
            }
            else
            {
                // If healthSlider is inside the range (0%, 25%] then fillColor is red
                if (healthSlider.value <= 0.25f * maxHealth && healthSlider.value > 0)
                {
                    fillColor.color = Color.red;
                }
                else
                {
                    // If healthSlider is zero then fillColor is transparent
                    if (healthSlider.value <= 0)
                    {
                        fillColor.color = new Color(0, 0, 0, 0);
                    }
                }
            }
        }

        // When the player falls down, the player dies
        if (transform.position.y < -10f)
        {
            // And because the player is not seen then only restart the scene
            GameController.instance.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }


    // Called when the enemy deals damage to the player
    public void TakeDamage(float damage)
    {
        // Decrease health and show the result at the slider
        health -= damage;

        SliderValue();
        StartCoroutine(HitReaction());
        
        // If the player health is less than 0, then the player dies
        if (health <= 0f)
        {
            Die();
        }   
    }

    // Damage taken when the head is inside the camera plane
    public void PenaltyDamage(float damage)
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

    // Activate hit reaction animation
    private IEnumerator HitReaction()
    {
        anim.SetBool(HashIDs.instance.hitBool, true);

        isHurt = true;

        yield return new WaitForSeconds(0.001f);

        anim.SetBool(HashIDs.instance.hitBool, false);
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
        RectTransform rectBar = healthSlider.gameObject.GetComponent<RectTransform>();
        rectBar.sizeDelta = new Vector2(maxHealth, rectBar.sizeDelta.y);
        healthSlider.maxValue = maxHealth;
    }

    // Modify the slider value
    private void SliderValue()
    {
        healthSlider.value = health;
    }

    // The player dies and the Dead animation is called
    private void Die()
    {
        if (!isDead)
        {
            // Animacion Die de prueba
            anim.SetBool(HashIDs.instance.deadBool, true);

            StartCoroutine(DeactivateCollider());
        }
    }

    // When the player dies deactivate the colliders of the GameObject so it doesn't
    // collide with anything
    private IEnumerator DeactivateCollider()
    {
        yield return new WaitForSeconds(0.01f);
        
        anim.SetBool(HashIDs.instance.deadBool, false);

        isDead = true;
        
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<CapsuleCollider>().enabled = false;

        // Wait 5 seconds and restart the level
        yield return new WaitForSeconds(5.0f);

        GameController.instance.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Called to set maxHealth from other scripts 
    public void SetMaxHealth(float maxHP)
    {
        maxHealth = maxHP;
        SliderSize();
    }

    public float GetHealth()
    {
        return health;
    }
    
}
