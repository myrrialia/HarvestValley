using UnityEngine;
using UnityEngine.UI;

//Manages the inventory UI
public class PlayerInvUI : InventoryUI
{
    [SerializeField] [Tooltip("Icon for the currently equipped item")] private Image equipIcon;  
    [SerializeField] [Tooltip("Text for the currently equipped item")] private Text equipText;
    [SerializeField] [Tooltip("Text for the player's total currency")] private Text currency;
    private PlayerInventory inventory;
    private ShopInvUI shopUI;

    //---Unity Methods---

    // Start is called before the first frame update
    void Start()
    {
        inventory = PlayerInventory.GetInstance();
        inventory.ItemChangedCallback += UpdateUI;
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        shopUI = gameObject.GetComponent<ShopInvUI>();
    }

    //---Custom Methods---

    //Opens/Closes the inventory window
    public override void ToggleInventoryUI()
    {
        base.ToggleInventoryUI();
        shopUI.ExitShop();  //Shop UI closes if inventory closes
    }

    //Called when there are changes to the inventory
    public override void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)  //Cycles through all the inv slots
        {
            if (i < inventory.items.Count)
                slots[i].AddItem(inventory.items[i]);
            else
                slots[i].ClearSlot();
        }
        //Update currency text
        currency.text = PlayerInventory.GetInstance().currency.ToString() + "g";    
    }

    //Update currently equipped item
    public void UpdateEquippedUI(InventoryItem item)
    {
        if(item)    //If there's an item equipped
        {
            equipText.text = item.name;
            equipIcon.sprite = item.icon;
            equipIcon.enabled = true;
        }
        else        //If there isn't
        {
            equipText.text = "";
            equipIcon.sprite = null;
            equipIcon.enabled = false;
        }
    }
}
