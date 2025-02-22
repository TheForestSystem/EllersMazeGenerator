using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private InputAction regenerateAction;

    private void Start()
    {
        regenerateAction = InputSystem.actions.FindAction("Regenerate");
        regenerateAction.performed += ctx => Regenerate();
    }

    private void Regenerate()
    {
        Debug.Log("Regenerating Maze");
        EllersMazeGenerator.Instance.RegenerateMaze();

    }

    private void OnDestroy()
    {
        regenerateAction.performed -= ctx => Regenerate(); // Unbind event
    }
}
