using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public UnityEvent<Vector2> OnMove;
    public UnityEvent<Vector2> OnLook;
    public UnityEvent OnStopMoving;
    public UnityEvent OnStopLooking;
    public UnityEvent OnInteract;
    public UnityEvent OnThrow;
    public UnityEvent OnJump;
    public UnityEvent OnRun;
    public UnityEvent OnStopRun;

    private PlayerInput plInput;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
        plInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDestroy()
    {
        if (Instance = this)
            Instance = null;
    }

    public void ExitCam(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (context.canceled)
            OnStopMoving?.Invoke();
        if (!context.performed)
            return;
        OnMove?.Invoke(context.ReadValue<Vector2>());
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        OnInteract?.Invoke();
    }

    public void Look(InputAction.CallbackContext context)
    {
        if (context.canceled)
            OnStopLooking?.Invoke();
        if (!context.performed)
            return;
        OnLook?.Invoke(context.ReadValue<Vector2>());
    }

    public void Throw(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        OnThrow?.Invoke();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        OnJump?.Invoke();
    }
    
    public void Run(InputAction.CallbackContext context)
    {
        if (context.canceled)
            OnStopRun?.Invoke();
        if (!context.performed)
            return;
        OnRun?.Invoke();
    }

    public void Deactivate() => plInput.DeactivateInput();

    public void Activate() => plInput.ActivateInput();
}
