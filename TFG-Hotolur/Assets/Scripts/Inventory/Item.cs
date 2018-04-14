using UnityEngine;

//[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {

	new public string name = "New Item";
    public Sprite icon = null;
    public bool isDefaultItem = false; // This will be deleted

    public virtual void Use()
    {
        // Use the item
    }

}
