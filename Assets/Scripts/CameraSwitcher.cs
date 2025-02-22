using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public static CameraSwitcher Instance { get; private set; }

    [SerializeField] private Camera playerCamera;
    private Camera topDownCamera;

    private void Awake()
    {
        Instance = this;
        topDownCamera = Camera.main;
    }

    private void Start()
    {
        playerCamera.enabled = true;
        topDownCamera.enabled = false;
    }

    public void SwitchCamera()
    {
        playerCamera.enabled = !playerCamera.enabled;
        topDownCamera.enabled = !topDownCamera.enabled;
    }
}
