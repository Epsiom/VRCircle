using UnityEngine;
using UnityEngine.InputSystem;

public class MouseControls : MonoBehaviour
{
    public Camera mainCamera; // Assign your main camera in the Inspector
    //public LayerMask interactionLayer; // Specify the layer for interactable objects
    //public float rayLength = 10f; // Max distance for raycasting

    private InputAction clickAction;
    //private InputAction moveAction;

    public float sensX = 400f;
    public float sensY = 400f;
    public float xRotation;
    public float yRotation;

    void Start()
    {
        if (UnityEngine.XR.XRSettings.isDeviceActive)
        {
            // Disable mouse interaction if VR is active
            this.enabled = false;
        }

        // Locks and hides the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Initialize Input Actions
        clickAction = new InputAction(binding: "<Mouse>/leftButton");
        //moveAction = new InputAction(binding: "<Mouse>/position");

        // Enable Input Actions
        clickAction.Enable();
        //moveAction.Enable();
    }

    void Update()
    {
        // Handle Mouse Movement for Look
        HandleMouseLook();

        // Handle Mouse Clicks
        if (clickAction.WasPressedThisFrame())
        {
            HandleMouseClick();
        }
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

        /*
        // Gets mouse movement delta (how far the mouse moved on each axis)
        float mouseX = Mouse.current.delta.x.ReadValue();  // Horizontal movement (left-right)
        float mouseY = Mouse.current.delta.y.ReadValue();  // Vertical movement (up-down)
        */
    }

    void HandleMouseClick()
    {
        Debug.Log("HandleMouseClick");

        /*
        // Raycast to detect interactable objects
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, rayLength, interactionLayer))
        {
            // Handle interaction logic with the object
            Debug.Log($"Interacted with {hit.collider.name}");
        }
        */
    }

    void OnDisable()
    {
        // Clean up Input Actions
        clickAction.Disable();
        //moveAction.Disable();
    }
}