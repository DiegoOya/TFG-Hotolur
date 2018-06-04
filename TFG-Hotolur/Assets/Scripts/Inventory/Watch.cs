using UnityEngine;

/// <summary>
/// ScriptableObject to label the variables and functions needed by a watch
/// </summary>
[CreateAssetMenu(fileName = "New Watch", menuName = "Inventory/Item/Watch")]
public class Watch : Item {

    // Time to stop the head
    public int stopTime;

    // Stop the head stopTime seconds
    public override void Use()
    {
        base.Use();

        // This will use the object and stop the head
        GameObject.FindGameObjectWithTag(Tags.head).GetComponent<HeadController>().StopHead(stopTime);
    }

}
