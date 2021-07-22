using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {        
        float _chargeTimer;
        Vector2 _movementInput;
        public Player _player;

        public Vector2 MovementInput => _movementInput;        

        private void Start()
        {
            _player = GetComponent<Player>();
        }

        public void OnMoveInput(InputAction.CallbackContext context)
        {
            _movementInput = context.ReadValue<Vector2>();
            _player.Movement.OnMovementChanged(_movementInput);
        }

        public void OnJumpInput(InputAction.CallbackContext context)
        {
            if (context.performed)
                _player.Movement.CheckJump();
        }

        public void OnThrowInput(InputAction.CallbackContext context)
        {
            if (context.performed)
                _player.Action.Throw(true);
            else if (context.canceled)
                _player.Action.Throw(false);

        }

        public void OnGrabInput(InputAction.CallbackContext context)
        {
            if (context.performed)
                _player.Action.Grab();
        }

        public void OnDashInput(InputAction.CallbackContext context)
        {
            if (context.performed)
                _player.Movement.Dash();
        }
    }
}
