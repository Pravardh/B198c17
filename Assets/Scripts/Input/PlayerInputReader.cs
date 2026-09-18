using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour, PlayerInput.ILocomotionActions
{
    private PlayerInput playerInput;

    public Vector2 PlayerMoveValue { get; private set; }
    public Vector2 PlayerLookValue { get; private set; }
    public bool IsRunning { get; private set; }

    public bool IsUsingMouseLook { get; private set; } = true;

    public event Action OnPlayerAttack;
    public event Action OnPlayerTarget;
    void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Locomotion.AddCallbacks(this);
        EnableInput();
    }

    void OnDestroy()
    {
        playerInput.Locomotion.RemoveCallbacks(this);
        playerInput.Dispose();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        PlayerMoveValue = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        PlayerLookValue = context.ReadValue<Vector2>();
        IsUsingMouseLook = context.control?.device is Mouse;
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        IsRunning = context.ReadValueAsButton();

    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

            OnPlayerAttack?.Invoke();
        }
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
    }
    public void OnTarget(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            OnPlayerTarget?.Invoke();
        }
    }

    public void EnableInput()
    {
        playerInput.Enable();
    }

    public void DisableInput()
    {
        playerInput.Disable();

        PlayerMoveValue = Vector2.zero;
        PlayerLookValue = Vector2.zero;
        IsRunning = false;
    }


}