using UnityEngine;

/// <summary>
/// Script used to manage when the player takes an item
/// </summary>
public class ItemPickUp : Interactable {

    // Variable used if the item will be used instantly *****PRIVATE(?)
    public bool useItemInstantly = false;

    // This variable is the ScriptableObject of the item
    [SerializeField]
    private Item item;

    // Called when the player can interact with the item
    public override void Interact()
    {
        // If the PickUp button is pressed the pick up the item
        base.Interact();
        if(Input.GetButtonDown("PickUp"))
            PickUp();
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
}
