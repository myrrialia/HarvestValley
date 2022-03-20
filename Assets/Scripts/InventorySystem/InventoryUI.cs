using UnityEngine;

//Manages the inventory UI
public abstract class InventoryUI : MonoBehaviour
{
    [SerializeField] [Tooltip("Inventory UI game object")] protected GameObject inventoryUI;
    [SerializeField] [Tooltip("The parent object of all the item slots")] protected Transform itemsParent;
    protected bool isActive;  //Is the inventory currently visible?
    protected InventorySlot[] slots;

    //Returns whether the inventory UI is active or not
    public bool InvIsActive()
    {
        return isActive;
    }

    //Opens/Closes the inventory window
    public virtual void ToggleInventoryUI()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
        isActive = inventoryUI.activeSelf;
        Cursor.lockState = isActive ? CursorLockMode.None: CursorLockMode.Locked;
        UpdateUI();
    }

    //Called when there are changes to the inventory
    public abstract void UpdateUI();
}
