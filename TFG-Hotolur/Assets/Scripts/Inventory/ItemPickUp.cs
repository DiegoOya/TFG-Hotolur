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

    private bool itemPickedUp = false;

    [SerializeField]
    private float volumeSounds = 0.7f;

    private Animator anim;

    private TextMeshProUGUI gotItemText;
    
    private void Start()
    {
        anim = player.GetComponent<Animator>();
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

            AudioSource.PlayClipAtPoint(item.sound, transform.position, volumeSounds);

            // If the weapon was picked up then deactivate it
            if (pickedUp)
            {
                transform.position = new Vector3(-50f, 0f, 0f);

                // If gotItemTextGO isn't null then show in the screen the object gotten, if not then search for it
                GameObject gotItemTextGO = GameObject.FindGameObjectWithTag(Tags.GotItemText);
                if (gotItemTextGO != null)
                {
                    gotItemText = gotItemTextGO.GetComponent<TextMeshProUGUI>();
                    gotItemText.text = string.Concat(item.name.ToString());
                    gotItemText.color = new Color(0, 1, 0, 1);
                    StartCoroutine(DeactivateText(gotItemText));
                }

                itemPickedUp = true;
            }
            
            return;
        }
        else
        {
            if (item is Potion)
            {
                // If gotItemTextGO isn't null then show in the screen the object gotten, if not then search for it
                GameObject gotItemTextGO = GameObject.FindGameObjectWithTag(Tags.GotItemText);
                if (gotItemTextGO != null)
                {
                    gotItemText = gotItemTextGO.GetComponent<TextMeshProUGUI>();
                    Potion potion = (Potion)item;
                    gotItemText.text = string.Concat("+", potion.percentageHealthToHeal.ToString(), "% HP");
                    gotItemText.color = new Color(0, 1, 0, 1);
                    StartCoroutine(DeactivateText(gotItemText));
                }
            }
            else
            {
                // If gotItemTextGO isn't null then show in the screen the object gotten, if not then search for it
                GameObject gotItemTextGO = GameObject.FindGameObjectWithTag(Tags.GotItemText);
                if (gotItemTextGO != null)
                {
                    gotItemText = gotItemTextGO.GetComponent<TextMeshProUGUI>();
                    Watch watch = (Watch)item;
                    gotItemText.text = string.Concat("+", watch.stopTime.ToString(), " s");
                    gotItemText.color = new Color(0, 1, 0, 1);
                    StartCoroutine(DeactivateText(gotItemText));
                }
            }
        }

        // Play the clip attached to the item
        AudioSource.PlayClipAtPoint(item.sound, transform.position, volumeSounds);

        itemPickedUp = true;
        transform.position = new Vector3(-50f, 0f, 0f);
        Destroy(gameObject, item.sound.length);
    }

    // Activate the pick up animation
    private IEnumerator PickUpObject()
    {
        // Activate the pick up animation
        anim.SetBool(HashIDs.instance.pickUpBool, true);

        yield return new WaitForSeconds(0.001f);

        // Deactivate the pick up animation, because it has exit time it will wait until the animation ends
        anim.SetBool(HashIDs.instance.pickUpBool, false);

        if (!itemPickedUp)
        {
            PickUp();
        }
    }

    // "Deactivate" the addPointsText by making it transparent modifying the alpha component
    private IEnumerator DeactivateText(TextMeshProUGUI text)
    {
        yield return new WaitForSeconds(item.sound.length * 0.8f);

        text.color = new Color(0, 0, 0, 0);
    }

    public Item GetItem()
    {
        return item;
    }

}
