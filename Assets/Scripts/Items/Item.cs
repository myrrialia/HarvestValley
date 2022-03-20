using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] [Tooltip("Item addded to inventory if picked up")] private InventoryItem item;

    //What to do when the item is interacted with
    public virtual void Interaction()
    {
        if (PlayerInventory.GetInstance().Add(item))   //If the item is successfully added to the inventory
        {
            FocusMarker.GetInstance().ResetFocused();
            Destroy(gameObject);    //Delete the item from the scene
        }
    }
}
