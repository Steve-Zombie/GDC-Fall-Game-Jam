using UnityEngine;
using UnityEngine.InputSystem;

// referenced off unity documentation charactercontroller.move
// and this https://www.youtube.com/watch?v=1uW-GbHrtQc
// and this https://discussions.unity.com/t/how-to-correctly-setup-3d-character-movement-in-unity/811250/2
public class Movement : MonoBehaviour
{

    [Header("Player Movement Settings")]
    [SerializeField] private float speed = 5.0f;
    //[SerializeField] private float sprintSpeed = 8.0f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -9.8f;

    public Camera playerCamera;
    public float lookSpeed = 2f;

    public float lookXLimit = 45f;


    private CharacterController controller;
    private Vector3 velocity;
    private bool onGround;
    private float rotationX;
    private float groundTimer;

    [Header("Input Actions")]
    public InputActionReference moveAction; // vector2
    public InputActionReference jumpAction; // button
    public InputActionReference sprintAction; // button
    public InputActionReference lookAction; // vector2

    private void Awake()
    {
        controller = gameObject.AddComponent<CharacterController>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        onGround = controller.isGrounded;
        if (onGround)
        {
            groundTimer = 0.2f;
        }
        if (groundTimer > 0)
        {
            groundTimer -= Time.deltaTime;
        }
        if (onGround && velocity.y < 0)
        {
            velocity.y = 0f;
        }
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        Vector3 move = transform.TransformDirection(Vector3.forward) * input.y + transform.TransformDirection(Vector3.right) * input.x;

        if (jumpAction.action.triggered && groundTimer > 0)
        {
            groundTimer = 0;
            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        Vector3 finalMove = (move * speed) + (velocity.y * Vector3.up);
        //if (sprintAction.action.triggered && groundTimer > 0) {
        //    finalMove = (move * speed) + (velocity.y * Vector3.up);
        //}
        //else {
        //    finalMove = (move * sprintSpeed) + (velocity.y * Vector3.up);
        //}

        Vector2 look = lookAction.action.ReadValue<Vector2>();
        controller.Move(finalMove * Time.deltaTime);

        rotationX += -look.y * lookSpeed;

        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        transform.rotation *= Quaternion.Euler(0, look.x * lookSpeed, 0);

    }
}
