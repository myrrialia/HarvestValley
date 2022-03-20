using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //---Components---
    [SerializeField] [Tooltip("Ground check transform")] private Transform groundCheck;
    [SerializeField] [Tooltip("Player inventory UI")] private PlayerInvUI inventoryUI;
    private CharacterController controller;
    new public Camera camera;
    private FocusMarker focusMarker;
    private InventoryItem equippedItem;  //Item the player currently has equipped

    //---Constants---
    const float GROUND_DIST = 0.4f;     //Margin of error for whether or not player is grounded
    const float STAND_HEIGHT = 2f;      //Player's height while standing
    const float CROUCH_HEIGHT = 1f;     //Player's height while crouching

    //---Variables---
    [SerializeField] [Tooltip("Ground layer mask")] private LayerMask groundMask;  //Ground layer
    [SerializeField] [Tooltip("How high the player can jump")] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;
    private float lookX, lookY = 0;     //Mouse x and y input
    private float hor, ver = 0;         //Horizontal and vertical movement input
    private float xRotation = 0f;       //Clamped camera rotation on the x-axis
    private float mouseSens = 0.8f; 
    private float speed = 10f;          //Player move speed
    private Vector3 velocity;           //Velocity applied to the player
    private bool isGrounded;            //Is the player touching the ground?
    private bool isCrouched = false;    //Is the player crouched?
    private float crouchSpeed = 3f;     //How quickly the player crouches/uncrouches
    
    //---Player Input Methods---

    //When the player tries to look around
    public void OnLook(InputAction.CallbackContext context)
    {
        if (!inventoryUI.InvIsActive())     //If the inventory UI isn't on screen
        {
            Vector2 val = context.ReadValue<Vector2>();
            lookX = val.x;
            lookY = val.y;
        }
    }

    //When the player tries to move
    public void OnMove(InputAction.CallbackContext context)
    {
        if (!inventoryUI.InvIsActive())     //If the inventory UI isn't on screen
        {
            Vector2 val = context.ReadValue<Vector2>();
            hor = val.x;
            ver = val.y;
        }
    }

    //When the player tries to jump
    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.performed && isGrounded && !inventoryUI.InvIsActive())
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    //When the player tries to crouch
    public void OnCrouch(InputAction.CallbackContext context)
    {
        if(!inventoryUI.InvIsActive())
        {
            if (context.performed)
            {
                isCrouched = true;
                StartCoroutine(Crouch());
            }
            else
            {
                isCrouched = false;
                StartCoroutine(Uncrouch());
            }
        }
    }

    //If there's an interactable in front of the player, interact with it
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && focusMarker.GetFocused() != null && !inventoryUI.InvIsActive())  
            focusMarker.GetFocused().Interaction(); 
    }

    //Opens and closes the inventory UI
    public void OnToggleInventory(InputAction.CallbackContext context)
    {
        if (context.started)
            inventoryUI.ToggleInventoryUI();
    }

    //Uses the currently equipped item
    public void OnUseTool(InputAction.CallbackContext context)
    {
        if (context.started && equippedItem && !inventoryUI.InvIsActive())
            equippedItem.Use();
    }

    //---Getters/Setters---

    public InventoryItem GetEquippedItem()
    {
        return equippedItem;
    }

    public void SetEquippedItem(InventoryItem item)
    {
            equippedItem = item;
            inventoryUI.UpdateEquippedUI(item);
    }

    //---Unity Methods---

    // Start is called before the first frame update
    private void Start()
    {
        controller = this.GetComponent<CharacterController>();  //Get reference to Character Controller
        focusMarker = this.GetComponentInChildren<FocusMarker>();   //Get reference to the focus marker
        camera = GetComponentInChildren<Camera>();  //Get reference to the main camera
        Cursor.lockState = CursorLockMode.Locked;   //Hide cursor
    }

    private void FixedUpdate()
    {
        //Camera movement
        xRotation -= lookY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);    //So player can't look behind themselves
        camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);  //Look up/down
        transform.Rotate(Vector3.up * lookX * mouseSens);  //Look right/left

        //Player movement
        Vector3 move = transform.right * hor + transform.forward * ver;
        controller.Move(move * speed * Time.deltaTime);

        //If we're touching the ground, reset velocity
        isGrounded = Physics.CheckSphere(groundCheck.position, GROUND_DIST, groundMask);
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;  //Feels better than actual 0

        //Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    //---Custom Methods---

    //Gradually shrinks controller height on crouch
    IEnumerator Crouch()
    {
        float t = 0f;
        while (isCrouched && controller.height != CROUCH_HEIGHT)
        {
            controller.height = Mathf.Lerp(STAND_HEIGHT, CROUCH_HEIGHT, t);
            t += crouchSpeed * Time.deltaTime;
            yield return null;
        }
    }

    //Gradually returns controller to original height on uncrouch
    IEnumerator Uncrouch()
    {
        float t = 0f;
        while (!isCrouched && controller.height != STAND_HEIGHT)
        {
            controller.height = Mathf.Lerp(CROUCH_HEIGHT, STAND_HEIGHT, t);
            t += crouchSpeed * Time.deltaTime;
            yield return null;
        }
    }
}
