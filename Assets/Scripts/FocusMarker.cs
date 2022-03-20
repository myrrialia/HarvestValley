using UnityEngine;

public class FocusMarker : MonoBehaviour
{
    //---Components---
    [SerializeField] [Tooltip("Focus marker material")] private Material green, blue, red;
    private static FocusMarker instance = null; //Reference to the Focus Marker
    private Player player;                      //Reference to the player
    private IInteractable focusedInteractable = null;   //Interactable we're currently looking at

    //---Variables---
    [SerializeField] [Tooltip("How many grid spaces from the player the marker can reach")] private int reach = 3;
    private bool occupied = false;              //Is something occupying the marker's tile

    //---Getters/Setters---

    public static FocusMarker GetInstance()
    {
        return instance;
    }

    public bool isOccupied()
    {
        return occupied;
    }

    //Returns the interactable currently in focus
    public IInteractable GetFocused()
    {
        return focusedInteractable;
    }

    //---Unity Methods---

    //Make sure there's only one instance of FocusMarker (Singleton)
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    private void Start()
    {
        player = GameManager.GetInstance().player;
    }

    private void Update()
    {
        Vector3 playerTrans = player.transform.position;
        //Offset from the player based off camera angle
        Vector3 offset = player.transform.forward * Mathf.Lerp(reach, 0f, (player.camera.transform.localEulerAngles.x / 90f));

        //Force the FocusMarker to grid
        transform.position = new Vector3(Mathf.Round((playerTrans + offset).x), 0.1f, Mathf.Round((playerTrans + offset).z));
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    ////Focus the interactable in range
    private void OnTriggerStay(Collider other)
    {
        occupied = true;
        focusedInteractable = other.GetComponent<IInteractable>();
        //If a plant is selected and it's harvestable, set marker to green
        if (other.gameObject.GetComponent<Plant>())  
        {
            if (other.gameObject.GetComponent<Plant>().isHarvestable()) 
                gameObject.GetComponentInChildren<MeshRenderer>().material = green; 
        }
        //If an item we can pick up is selected, set marker to green
        else if (other.GetComponent<Item>())     
            gameObject.GetComponentInChildren<MeshRenderer>().material = green;
        else
            //Else set marker to red
            gameObject.GetComponentInChildren<MeshRenderer>().material = red;   
    }

    //Unfocus the interactable when it's no longer in range
    private void OnTriggerExit(Collider other)
    {
        occupied = false;
        if (focusedInteractable == other.GetComponent<IInteractable>())
            focusedInteractable = null;
        gameObject.GetComponentInChildren<MeshRenderer>().material = blue;  //Set marker to blue
    }

    //---Custom Methods---

    //Reset the interactable currently in focus
    public void ResetFocused()
    {
        focusedInteractable = null;
        occupied = false;
        gameObject.GetComponentInChildren<MeshRenderer>().material = blue;  //Set marker to blue
    }
}
