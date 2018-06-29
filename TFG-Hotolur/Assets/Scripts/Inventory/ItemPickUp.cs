using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Script used to manage when the player takes an item
/// </summary>
public class ItemPickUp : Interactable {
    
    // This variable is the ScriptableObject of the item
    [SerializeField]
    private Item item;

    private Animator anim;

    private TextMeshProUGUI gotItemText;

    private AudioSource audioSource;

    private void Start()
    {
        anim = player.GetComponent<Animator>();

        audioSource = player.GetComponents<AudioSource>()[2];
    }

    // Called when the player can interact with the item
    public override void Interact()
    {
        // If the PickUp button is pressed the pick up the item
        StartCoroutine(PickUpObject());
    }

    // Pick up the object and add it to the inventory or use it instantly
    void PickUp()
    {
        Debug.Log("Picking up " + item.name);
        
        // Use the item
        item.Use();

        // If it is the weapon it is treated differently
        if (item is Weapon)
        {
            bool pickedUp = Inventory.instance.Add(item);

            audioSource.clip = item.sound;
            audioSource.Play();

            // If the weapon was picked up then deactivate it
            if (pickedUp)
            {
                gameObject.SetActive(false);
            }

            // If gotItemTextGO isn't null then show in the screen the object gotten, if not then search for it
            GameObject gotItemTextGO = GameObject.FindGameObjectWithTag(Tags.addPointsText);
            if (gotItemTextGO != null)
            {
                gotItemText = gotItemTextGO.GetComponent<TextMeshProUGUI>();
                gotItemText.text = string.Concat(item.name.ToString());
            }

            return;
        }
        else
        {
            if (item is Potion)
            {
                // If gotItemTextGO isn't null then show in the screen the object gotten, if not then search for it
                GameObject gotItemTextGO = GameObject.FindGameObjectWithTag(Tags.addPointsText);
                if (gotItemTextGO != null)
                {
                    gotItemText = gotItemTextGO.GetComponent<TextMeshProUGUI>();
                    Potion potion = (Potion)item;
                    gotItemText.text = string.Concat("+", potion.percentageHealthToHeal.ToString(), " HP");
                }
            }
            else
            {
                // If gotItemTextGO isn't null then show in the screen the object gotten, if not then search for it
                GameObject gotItemTextGO = GameObject.FindGameObjectWithTag(Tags.addPointsText);
                if (gotItemTextGO != null)
                {
                    gotItemText = gotItemTextGO.GetComponent<TextMeshProUGUI>();
                    Watch watch = (Watch)item;
                    gotItemText.text = string.Concat("+", watch.stopTime.ToString(), " s");
                }
            }
        }

        // If it is another item then play the sound and destroy it
        audioSource.clip = item.sound;
        audioSource.Play();

        Destroy(gameObject);
    }

    // Activate the pick up animation
    private IEnumerator PickUpObject()
    {
        // Activate the pick up animation
        anim.SetBool(HashIDs.instance.pickUpBool, true);

        yield return new WaitForSeconds(0.001f);

        // Deactivate the pick up animation, because it has exit time it will wait until the animation ends
        anim.SetBool(HashIDs.instance.pickUpBool, false);

        PickUp();
    }

    public Item GetItem()
    {
        return item;
    }

}
