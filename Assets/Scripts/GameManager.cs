using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] [Tooltip("Shop inventory UI")] private ShopInvUI shopUI;
    private static GameManager instance = null;  //Reference to the GameManager
    public Player player;  //Reference to the player

    //Make sure there's only one instance of GameManager (Singleton)
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public static GameManager GetInstance()
    {
        return instance;
    }

    public bool shopIsActive()
    {
        return shopUI.InvIsActive();
    }
}
