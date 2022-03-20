using UnityEngine;

public class ShopStand : MonoBehaviour, IInteractable
{
    [SerializeField] [Tooltip("Player inventory UI")] private PlayerInvUI invUI;
    [SerializeField] [Tooltip("Shop inventory UI")] private ShopInvUI shopUI;

    //When the player interacts with the shop stand
    public void Interaction()
    {
        invUI.ToggleInventoryUI();  //Toggle both UI
        shopUI.ToggleInventoryUI();
    }
}
