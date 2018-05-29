using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script used to manage the inventory of the player
/// </summary>
public class Inventory : MonoBehaviour {

    #region Singleton
    public static Inventory instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one instance of Inventory");
            return;
        }
                
        instance = this;
    }

    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged OnItemChangedCallBack;

    // Space of the inventory
    public int space = 20;

    // List of the items kept in the inventory
    [SerializeField]
    private List<Item> items = new List<Item>();

    // Called when an object is added to the inventory
    public bool Add (Item item)
    {
        // If there are no more space
        if(items.Count >= space)
        {
            // It writes a message to the console and do not add the item
            Debug.Log("Not enough room");
            return false;
        }

        // Add the item to the list
        items.Add(item);

        if(OnItemChangedCallBack != null)
            OnItemChangedCallBack.Invoke();

        return true;
    }

    // Called when the item in the inventory is used or removed
    public void Remove(Item item)
    {
        // Remove the item in the list
        items.Remove(item);

        if (OnItemChangedCallBack != null)
            OnItemChangedCallBack.Invoke();
    }

}
