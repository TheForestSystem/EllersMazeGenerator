using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private InputAction regenerateAction;
    private InputAction switchViewAction;
    private InputAction lookAction;
    private InputAction moveAction;
    private InputAction quitAction;

    private Vector2 lookDirection;
    private Vector2 moveDirection;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        regenerateAction = InputSystem.actions.FindAction("Regenerate");
        switchViewAction = InputSystem.actions.FindAction("SwitchView");
        lookAction = InputSystem.actions.FindAction("Look");
        moveAction = InputSystem.actions.FindAction("Move");
        quitAction = InputSystem.actions.FindAction("Quit");

        regenerateAction.performed += ctx => Regenerate();
        switchViewAction.performed += ctx => SwitchView();
        quitAction.performed += ctx => Application.Quit();

        lookAction.started += OnLook;
        lookAction.performed += OnLook;
        lookAction.canceled += OnLookCanceled;

        moveAction.started += OnMove;
        moveAction.performed += OnMove;
        moveAction.canceled += OnMoveCanceled;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
        // Debug.Log("Moving: " + moveDirection);
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveDirection = Vector2.zero;
        // Debug.Log("Move Stopped");
    }

    private void OnLook(InputAction.CallbackContext ctx)
    {
        lookDirection = ctx.ReadValue<Vector2>();
        // Debug.Log("Looking: " + lookDirection);
    }

    private void OnLookCanceled(InputAction.CallbackContext ctx)
    {
        lookDirection = Vector2.zero;
        // Debug.Log("Look Stopped");
    }

    private void SwitchView()
    {
        Debug.Log("Switching Camera");
        CameraSwitcher.Instance.SwitchCamera();
    }

    private void Regenerate()
    {
        Debug.Log("Regenerating Maze");
        EllersMazeGenerator.Instance.RegenerateMaze();
    }

    public float[] GetLookDirection()
    {
        return new float[] { lookDirection.x, lookDirection.y };
    }

    public float[] GetMoveDirection()
    {
        return new float[] { moveDirection.x, moveDirection.y };
    }

    private void OnDestroy()
    {
        regenerateAction.performed -= ctx => Regenerate();
        switchViewAction.performed -= ctx => SwitchView();

        lookAction.started -= OnLook;
        lookAction.performed -= OnLook;
        lookAction.canceled -= OnLookCanceled;
    }
}
