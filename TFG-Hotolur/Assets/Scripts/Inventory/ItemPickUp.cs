using System.Collections;
using UnityEngine;

/// <summary>
/// Script used to manage when the player takes an item
/// </summary>
public class ItemPickUp : Interactable {

    // Variable used if the item will be used instantly *****PRIVATE(?)
    public bool useItemInstantly = false;

    // Pick up animation of the player
    // If you want to change the pickUpAnim, you have to delete [HideInInspector] temporally
    [HideInInspector]
    public AnimationClip pickUpAnim;

    // Length of pickUpAnim
    private float timePickUpAnim;

    // This variable is the ScriptableObject of the item
    [SerializeField]
    private Item item;

    private Animator anim;

    private void Start()
    {
        timePickUpAnim = pickUpAnim.length;
        anim = player.GetComponent<Animator>();
    }

    // Called when the player can interact with the item
    public override void Interact()
    {
        // If the PickUp button is pressed the pick up the item
        base.Interact();
        if (Input.GetButtonDown("PickUp"))
            StartCoroutine(PickUpObject());
    }

    // Pick up the object and add it to the inventory or use it instantly
    void PickUp()
    {
        Debug.Log("Picking up " + item.name);

        // Add to the inventory or use it instantly
        // This is for Debug, when we know we like one or the other we leave only one
        bool pickedUp = false;
        if (useItemInstantly)
        {
            // What does the item do?
            item.Use();
        }
        else
        {
            pickedUp = Inventory.instance.Add(item);
        }

        // If the object was picked up then destroy the object
        if(pickedUp)
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
}
