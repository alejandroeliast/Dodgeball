using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    // Movement and Aim
    Vector2 _movementInput;
    Vector2 _aimInput;

    public int Index { get; private set; }
    public bool IsAssigned { get; set; }
    public Vector2 MovementInput => _movementInput;
    public string ControllerType { get; private set; }

    public Character character { get; set; }
    internal void SetIndex(int index)
    {
        Index = index;
        gameObject.name = "Controller " + Index;
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<Vector2>();
        character?.Movement.OnMovementChanged(_movementInput);
    }
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            character?.Movement.CheckJump();
        }
    }
    public void OnThrowInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            character?.Action.Throw(true);
        else if (context.canceled)
            character?.Action.Throw(false);

        ControllerType = context.control.parent.displayName;
    }
    public void OnGrabInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            character?.Action.Grab();
    }
    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            character?.Movement.Dash();
    }
    public void OnAimInput(InputAction.CallbackContext context)
    {
        _aimInput = context.ReadValue<Vector2>();
        character?.Action.OnAimChanged(_aimInput);
    }
    public void OnSwitchLeftInput(InputAction.CallbackContext context)
    {
        if (context.started)        
            character?.Action.MoveListBackward();        
    }
    public void OnSwitchRightInput(InputAction.CallbackContext context)
    {
        if (context.started)        
            character?.Action.MoveListForward();        
    }
}