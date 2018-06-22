using System.Collections;
using UnityEngine;

/// <summary>
/// Script used to manage when the player takes an item
/// </summary>
public class ItemPickUp : Interactable {
    
    // Pick up animation of the player
    // If you want to change the pickUpAnim, you have to delete [HideInInspector] temporally
    [HideInInspector]
    public AnimationClip pickUpAnim;

    // Length of pickUpAnim
    private float timePickUpAnim;

    // Variable used if the item will be used instantly 
    //[SerializeField]
    //private bool useItemInstantly = false;

    // This variable is the ScriptableObject of the item
    [SerializeField]
    private Item item;

    private Animator anim;

    private AudioSource audioSource;

    private void Start()
    {
        timePickUpAnim = pickUpAnim.length;
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

            return;
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

        // Wait until the player picks the object in the animation
        yield return new WaitForSeconds(timePickUpAnim / (2f * anim.GetNextAnimatorStateInfo(1).speed));

        // If the player is not in the pick up state, then interrupt the pick up action,
        // i.e., the player does not pick the object
        // But if it is still in the same animation, then pick up the object
        if (anim.GetNextAnimatorStateInfo(1).fullPathHash == HashIDs.instance.pickUpState)
        {
            PickUp();
        }
    }

    public Item GetItem()
    {
        return item;
    }

}
