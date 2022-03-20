using UnityEngine;

public class PlayerInventory : Inventory
{
    private static PlayerInventory instance = null;  //Reference to the player inventory
    private int invSize = 20;   //Max number of different items player can carry
    public int currency = 0;    //How much currency the player has

    //Make sure there's only one instance of PlayerInventory (Singleton)
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public static PlayerInventory GetInstance()
    {
        return instance;
    }

    //Adds an item to the inventory if there's room
    public bool Add (InventoryItem item)
    {
        if (items.Count >= invSize) //If the inventory is full
        {
            Debug.Log("Inventory is full!");
            return false;   //Failed to pick up item
        }
        items.Add(item);    //If there's room, add the item and invoke the delegate
        if (ItemChangedCallback != null)
            ItemChangedCallback.Invoke();
        return true;    //Successfully picked up item
    }
}
