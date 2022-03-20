using System.Collections.Generic;
using UnityEngine;

public abstract class Inventory : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>();  //List of items currently in the inventory
    public delegate void ItemChanged();     //Delegate to call when an item in the inventory has changed
    public ItemChanged ItemChangedCallback;

    //Removes an item from the inventory
    public void Remove(InventoryItem item)
    {
        items.Remove(item);
        if (ItemChangedCallback != null)
            ItemChangedCallback.Invoke();
    }
}
