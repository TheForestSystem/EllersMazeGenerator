using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float sensitivityX = 40.0f;
    [SerializeField] private float sensitivityY = 40.0f;

    [SerializeField] private Transform orientation;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    private InputManager inputManager;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        inputManager = InputManager.Instance;
    }

    private void Update()
    {
        float[] rawLookDirection = inputManager.GetLookDirection();

        float mouseX = rawLookDirection[0] * Time.deltaTime * sensitivityX;
        float mouseY = rawLookDirection[1] * Time.deltaTime * sensitivityY;

        rotationY += mouseX;
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90.0f, 90.0f);

        // Rotate the Player Camera and Orientation
        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        orientation.rotation = Quaternion.Euler(0, rotationY, 0);
    }
}
