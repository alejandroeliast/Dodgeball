using UnityEngine;

namespace OldPlayer
{
    public class PlayerMovement : MonoBehaviour
    {
        #region Variables
        // References
        [SerializeField] PlayerController _player;
        Rigidbody2D _rigidbody2D;

        // Horizontal
        [Header("Horizontal Movement")]
        [SerializeField] float _moveSpeed;
        [SerializeField] float _acceleration;
        [SerializeField] float _breaking;
        [SerializeField] float _airAcceleration;
        [SerializeField] float _airBreaking;

        Vector2 _movementVector;

        // Jump
        [Header("Jump")]
        [SerializeField] float _jumpForce;
        [SerializeField] int _maxJumps = 1;
        [SerializeField] float _downPull = 5f;
        [SerializeField] Vector2 _wallJumpForce;

        int _jumpsRemaining;
        float _jumpTimer;
        float _fallTimer;

        // Wall Slide
        [Header("Wall Slide")]
        [SerializeField] float _wallSlideSpeed;

        // Dash
        [Header("Dash")]
        [SerializeField] float _dashTime;
        [SerializeField] float _dashSpeed;
        [SerializeField] float _dashCooldown;

        private float _dashTimeLeft;
        private float _lastDash = -100f;

        // Facing Variables
        bool isFacingRight = true;
        int facingDirection = 1;

        // State Variables
        bool _isRunning = false;
        bool _isWallSliding = false;
        bool _isCrouched = false;
        bool _isDashing;
        private bool _isWallJumping;
        #endregion

        void Start()
        {
            _player = GetComponent<PlayerController>();
            _rigidbody2D = _player.Rigidbody2D;
        }

        public void OnMovementChanged(Vector2 vector)
        {
            _movementVector = vector;

            if (ShouldCrouch())
                CrouchStart();
            else
                CrouchEnd();
        }

        void Update()
        {
            CheckMovementDirection();
        }
        void FixedUpdate()
        {
            CheckHorizontalMovement();
            CheckVerticalMovement();
            CheckWallSlide();
            CheckDash();
        }

        #region Horizontal Movement        
        void CheckHorizontalMovement()
        {
            if (ShouldMoveHorizontally())
                HorizontalMoveStart();
            else
                HorizontalMoveEnd();

            _player.Animator.SetFloat("Horizontal", Mathf.Abs(_movementVector.x));
        }
        bool ShouldMoveHorizontally()
        {
            if (_isCrouched)
                return false;

            if (_movementVector.x == 0)
                return false;

            return true;
        }
        void HorizontalMoveStart()
        {
            float smoothnessMultiplier = _movementVector.x == 0 ? _breaking : _acceleration;
            if (_player.Collision.IsGrounded == false)
            {
                smoothnessMultiplier = _movementVector.x == 0 ? _airBreaking : _airAcceleration;
            }

            float newHorizontal = Mathf.Lerp(
                _rigidbody2D.velocity.x,
                _movementVector.x * _moveSpeed,
                Time.deltaTime * smoothnessMultiplier);

            _rigidbody2D.velocity = new Vector2(newHorizontal, _rigidbody2D.velocity.y);

            //_rigidbody2D.velocity = new Vector2(_movementVector.x * _moveSpeed, _rigidbody2D.velocity.y);

            if (_isRunning)
                return;

            _isRunning = true;
            _player.Animator.SetBool("Run", _isRunning);
        }
        void HorizontalMoveEnd()
        {
            _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);

            if (!_isRunning)
                return;

            _isRunning = false;
            _player.Animator.SetBool("Run", _isRunning);
        }
        #endregion

        #region Vertical Movement
        void CheckVerticalMovement()
        {
            _jumpTimer += Time.deltaTime;

            if (_player.Collision.IsGrounded)
            {
                if (_fallTimer > 0)
                {
                    _fallTimer = 0;
                    _jumpsRemaining = _maxJumps;
                    _isWallJumping = false;
                }
                if (_player.Animator.GetFloat("Vertical") <= 0)
                    _player.Animator.SetFloat("Vertical", 0);
            }
            else
            {
                if (_isWallSliding)
                    return;

                _fallTimer += Time.deltaTime;
                var downForce = _downPull * _fallTimer * _fallTimer;
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y - downForce);

                _player.Animator.SetFloat("Vertical", _rigidbody2D.velocity.y);
            }
        }
        #endregion

        #region Jump
        public void CheckJump()
        {
            if (ShouldJump())
            {
                if (!_isWallSliding)
                {
                    _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpForce);
                    _jumpsRemaining--;
                    _fallTimer = 0;
                    _jumpTimer = 0;

                    if (_isCrouched)
                        CrouchEnd();
                }
            }

            if (ShouldWallJump())
            {
                _rigidbody2D.velocity = new Vector2(-_movementVector.x * _wallJumpForce.x, _wallJumpForce.y);
                _isWallJumping = true;
            }
        }
        public void CancelJump()
        {
            _isWallJumping = false;
        }
        bool ShouldJump()
        {
            if (!_player.Collision.IsGrounded)
                return false;

            return true;
        }
        bool ShouldWallJump()
        {
            if (_player.Collision.IsGrounded)
                return false;

            if (!_isWallSliding)
                return false;

            return true;
        }
        #endregion

        #region Wall Slide
        void CheckWallSlide()
        {
            if (ShouldWallSlide())
            {
                if (!_isWallSliding)
                    _isWallSliding = true;

                if (!_isWallJumping)
                {
                    if (_player.Rigidbody2D.velocity.y > 0)
                    {
                        _player.Rigidbody2D.velocity = Vector2.zero;
                    }

                    _fallTimer = 0;
                    _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, -_wallSlideSpeed);
                }

                if (!_player.Animator.GetBool("WallSlide"))
                    _player.Animator.SetBool("WallSlide", true);
            }
            else
            {
                _isWallJumping = false;

                if (_isWallSliding)
                    _isWallSliding = false;

                if (_player.Animator.GetBool("WallSlide"))
                    _player.Animator.SetBool("WallSlide", false);
            }
        }
        bool ShouldWallSlide()
        {
            if (!_player.Collision.IsTouchingWall)
                return false;

            if (_player.Collision.IsGrounded)
                return false;

            if ((isFacingRight && _movementVector.x <= 0) || (!isFacingRight && _movementVector.x >= 0))
                return false;

            if (_player.Action.IsThrowing)
                return false;

            return true;
        }
        #endregion

        #region Crouch
        bool ShouldCrouch()
        {
            if (_player.Input.MovementInput.y >= 0)
                return false;

            if (Mathf.Abs(_player.Input.MovementInput.y) < Mathf.Abs(_player.Input.MovementInput.x))
                return false;

            if (!_player.Collision.IsGrounded)
                return false;

            return true;
        }
        void CrouchStart()
        {
            if (_isCrouched)
                return;

            _player.Collision.SetColliderOffset(0f, 0.5f);
            _player.Collision.SetColliderSize(0.5f, 0.85f);

            _isCrouched = true;

            if (!_player.Animator.GetBool("Crouch"))
                _player.Animator.SetBool("Crouch", _isCrouched);
        }
        void CrouchEnd()
        {
            _player.Collision.SetColliderOffset(0f, 0.7f);
            _player.Collision.SetColliderSize(0.5f, 1.25f);

            _isCrouched = false;
            if (_player.Animator == true)
                _player.Animator.SetBool("Crouch", _isCrouched);
        }
        #endregion

        #region Dash
        public void Dash()
        {
            if (ShouldDash())
            {
                _isDashing = true;
                _dashTimeLeft = _dashTime;
                _lastDash = Time.deltaTime;

                _player.Animator.SetBool("Dash", _isDashing);

                CrouchEnd();

                _player.Collision.SetColliderOffset(0f, 0.5f);
                _player.Collision.SetColliderSize(0.5f, 0.85f);
            }
        }
        bool ShouldDash()
        {
            if (!_player.Collision.IsGrounded)
                return false;

            if (!_isCrouched)
                return false;

            if (Time.time < (_lastDash + _dashCooldown))
                return false;

            return true;
        }
        void CheckDash()
        {
            if (_isDashing)
            {
                if (_dashTimeLeft > 0)
                {
                    _rigidbody2D.velocity = new Vector2(_dashSpeed * facingDirection, _rigidbody2D.velocity.y);
                    _dashTimeLeft -= Time.deltaTime;
                }

                if (_dashTimeLeft <= 0)
                {
                    _isDashing = false;
                    _player.Animator.SetBool("Dash", _isDashing);

                    if (ShouldCrouch())
                        CrouchStart();
                }
            }
        }
        #endregion

        void CheckMovementDirection()
        {
            if (isFacingRight && _player.Input.MovementInput.x < 0)
            {
                Flip();
            }
            else if (!isFacingRight && _player.Input.MovementInput.x > 0)
            {
                Flip();
            }
        }
        void Flip()
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }
}