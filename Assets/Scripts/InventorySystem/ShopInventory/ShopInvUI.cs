
public class ShopInvUI : InventoryUI
{
    private ShopInventory inventory;

    //---Unity Methods---

    // Start is called before the first frame update
    void Start()
    {
        inventory = ShopInventory.GetInstance();
        inventory.ItemChangedCallback += UpdateUI;
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    //---Custom Methods---

    //Called when there are changes to the inventory
    public override void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
                slots[i].AddItem(inventory.items[i]);
            else
                slots[i].ClearSlot();
        }
    }

    //Closes the shop UI
    public void ExitShop()
    {
        inventoryUI.SetActive(false);
        isActive = false;
    }
}
