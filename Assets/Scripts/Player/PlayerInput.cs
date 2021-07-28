using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        // Player Script Reference
        [SerializeField] Player _player;

        // Movement and Aim
        Vector2 _movementInput;
        Vector2 _aimInput;

        public Vector2 MovementInput => _movementInput;
        public string ControllerType { get; private set; }

        void Start()
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

            ControllerType = context.control.parent.displayName;
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
        public void OnAimInput(InputAction.CallbackContext context)
        {
            _aimInput = context.ReadValue<Vector2>();
            _player.Action.OnAimChanged(_aimInput);
        }
    }
}
