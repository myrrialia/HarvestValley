using UnityEngine.UI;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Button removeButton;
    [SerializeField] private Image icon;  
    private InventoryItem item; //Item currently stored in the slot

    //Clears the slot
    public virtual void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        if(removeButton)
            removeButton.interactable = false;
    }

    //Adds item to the slot
    public void AddItem (InventoryItem newItem)
    {
        item = newItem;
        icon.sprite = item.icon;    
        icon.enabled = true;
        if (removeButton)
            removeButton.interactable = true;
    }

    //Remove the item from the player's inventory
    public void RemoveItem()
    {
        item.Drop();
    }

    //When an item is clicked in the player inventory
    public void SelectItem()
    {
        if(GameManager.GetInstance().shopIsActive())    //If Shop is open, sell item
            SellItem();
        else if (item)      //Otherwise, equip item
            item.Equip();
    }

    //Add item to player's inventory if they have sufficient currency
    public void BuyItem()
    {
        if(item)
        {
            PlayerInventory playerInv = PlayerInventory.GetInstance();
            if (playerInv.currency >= item.GetValue())
            {
                playerInv.currency -= item.GetValue();
                playerInv.Add(item);
            }
            else
                Debug.Log("Insufficient funds");
        }
    }

    //Remove the item from the inventory and add its value to the player's currency
    public void SellItem()
    {
        if(item)
        {
            if (GameManager.GetInstance().player.GetEquippedItem() == item) //Reset equipped item if sold
                GameManager.GetInstance().player.SetEquippedItem(null); 
            PlayerInventory.GetInstance().currency += item.GetValue();
            PlayerInventory.GetInstance().Remove(item);
        }
    }
}
