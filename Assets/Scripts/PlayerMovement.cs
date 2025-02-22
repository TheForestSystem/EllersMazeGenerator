using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private float moveForce = 5.0f;

    [SerializeField] private float groundDrag;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight = 2.0f;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;

    [Header("References")]
    [SerializeField] private Transform orientation;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;

    private Rigidbody rb;

    private InputManager inputManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        inputManager = InputManager.Instance;
    }

    private void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        FetchInput();
        SpeedControl();

        // Handle drag
        if (isGrounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0.0f;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void FetchInput()
    {
        float[] rawMoveDirection = inputManager.GetMoveDirection();

        horizontalInput = rawMoveDirection[0];
        verticalInput = rawMoveDirection[1];
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDirection.normalized * movementSpeed * moveForce, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        if (flatVel.magnitude > movementSpeed)
        {
            flatVel = flatVel.normalized * movementSpeed;
            rb.linearVelocity = new Vector3(flatVel.x, rb.linearVelocity.y, flatVel.z);
        }
    }
}
