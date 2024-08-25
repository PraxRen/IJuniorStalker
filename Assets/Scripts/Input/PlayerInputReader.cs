using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour, PlayerInput.IPlayerActions
{
    [SerializeField] private bool _cursorLocked = true;

    private PlayerInput _playerInput;

    public event Action Jump;
    public event Action<bool> Run;

    public Vector2 DirectionMove { get; private set; }
    public Vector2 Look { get; private set; }
    public bool IsRun { get; private set; }

    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(_cursorLocked);
    }

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _playerInput.Player.SetCallbacks(this);
    }

    private void OnEnable()
    {
        _playerInput.Player.Enable();
        SetCursorState(_cursorLocked);
    }

    private void OnDisable()
    {
        _playerInput.Player.Disable();
        SetCursorState(false);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        DirectionMove = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Look = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed == false)
            return;

        Jump?.Invoke();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        IsRun = context.ReadValueAsButton();
        Run?.Invoke(IsRun);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}