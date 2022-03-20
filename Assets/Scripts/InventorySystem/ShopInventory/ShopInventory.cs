
public class ShopInventory : Inventory
{
    private static ShopInventory instance = null;  //Reference to the shop inventory

    //Make sure there's only one instance of ShopInventory (Singleton)
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public static ShopInventory GetInstance()
    {
        return instance;
    }
}
