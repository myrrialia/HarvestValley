using UnityEngine;

[CreateAssetMenu(fileName = "New Seed", menuName = "Inventory/Seed")]

public class Seed : InventoryItem
{
    [SerializeField] [Tooltip("Prefab to spawn when planted")] private GameObject plant;   
    [SerializeField] [Tooltip("Necessary Y-axis value so plant is level with the ground")] private float yOffset;

    //Plant seed at rounded marker location
    public override void Use()
    {
        if(!FocusMarker.GetInstance().isOccupied())
        {
            Vector3 plantLocation = FocusMarker.GetInstance().transform.position;
            Instantiate(plant, new Vector3(Mathf.Round(plantLocation.x), yOffset, Mathf.Round(plantLocation.z)), Quaternion.Euler(0f, 0f, 0f));
            PlayerInventory.GetInstance().Remove(this); //Remove seed from inventory
            GameManager.GetInstance().player.SetEquippedItem(null); //Reset equipped item
        }
    }
}
