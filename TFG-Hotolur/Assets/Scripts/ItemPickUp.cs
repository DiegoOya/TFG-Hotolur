using UnityEngine;

public class ItemPickUp : Interactable {

    public bool useItemInstantly = false;

    public Item item;

    public override void Interact()
    {
        base.Interact();
        if(Input.GetButtonDown("PickUp"))
            PickUp();
    }

    void PickUp()
    {
        Debug.Log("Picking up " + item.name);

        // Add to the inventory or use it instantly
        // This is for Debug, when we know we like one or the other we leave only one
        bool pickedUp = false;
        if (useItemInstantly)
        {
            // What does the item do?
        }
        else
        {
            pickedUp = Inventory.instance.Add(item);
        }

        if(pickedUp)
            Destroy(gameObject);
    }
}
