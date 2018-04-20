using UnityEngine;

//[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
/// <summary>
/// ScriptableObject used to label the variables and functions needed by an item
/// </summary>
public class Item : ScriptableObject {

	new public string name = "New Item";
    public Sprite icon = null;
    public bool isDefaultItem = false; // This will be deleted

    // Called when an item is used
    public virtual void Use() {}

}
