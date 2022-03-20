using UnityEngine;

public abstract class InventoryItem : ScriptableObject
{
    new public string name = "New Item";    //Overwrites name variable
    public Sprite icon = null;
    [SerializeField] [Tooltip("Game object to spawn if dropped")] private GameObject item; 
    [SerializeField] [Tooltip("Can the item be equipped?")] private bool equippable;
    [SerializeField] [Tooltip("Value of the item in the shop")] private int value;

    //Returns the value of the item
    public int GetValue()
    {
        return value;
    }

    //When the item is equipped from the inventory
    public void Equip()
    {
        if (equippable)
            GameManager.GetInstance().player.SetEquippedItem(this); //Set it as the currently equipped item
    }

    //Drop this item from the inventory
    public void Drop()
    {
        if (item)
        {
            if (GameManager.GetInstance().player.GetEquippedItem() == this)
                GameManager.GetInstance().player.SetEquippedItem(null);
            //Round the drop location so it's within a single grid tile
            Vector3 dropLocation = GameManager.GetInstance().player.transform.position;
            Instantiate(item, new Vector3(Mathf.Round(dropLocation.x), dropLocation.y, Mathf.Round(dropLocation.z)), Quaternion.Euler(0f, 0f, 0f));
            PlayerInventory.GetInstance().Remove(this);
        }
        else
            PlayerInventory.GetInstance().Remove(this); //Permanently deletes item
    }

    //Behavior when used from equipped slot
    public abstract void Use();
}
