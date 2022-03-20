using UnityEngine;

[CreateAssetMenu(fileName = "New Seed", menuName = "Inventory/Crop")]

public class Crop : InventoryItem
{
    public override void Use()
    {
        //To be implemented in the future - Could restore energy or produce more seeds
        Debug.Log("Using crop");
    }
}
