using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerCollision : MonoBehaviour
    {
        #region Variables
        // Main Player Script reference
        Player _player;

        // Ground Check
        [Header("Ground Check")]
        [SerializeField] float _groundCheckRadius;
        [SerializeField] Transform _groundCheck;
        [SerializeField] LayerMask _whatIsGround;
        public bool IsGrounded { get; private set; }

        // Wall Check
        [Header("Wall Check")]
        [SerializeField] Vector2 _wallCheckSize;
        [SerializeField] Transform _wallCheck;
        [SerializeField] LayerMask _whatIsWall;
        public bool IsTouchingWall { get; private set; }

        // Grab Check
        [Header("Grab Check")]
        [SerializeField] Transform _grabJoint;
        [SerializeField] ArrowFollow _selectArrow;
        [SerializeField] LayerMask _closestLayerMask;

        float _closestDistance = 99999f;
        Transform _closest = null;
        HashSet<Collider2D> _closestColliders = new HashSet<Collider2D>();

        public Transform Closest => _closest;
        #endregion

        void Start()
        {
            _player = GetComponent<Player>();
            _closestLayerMask = LayerMask.GetMask("BallPassive");
        }

        #region Set Collider Parameters
        public void SetColliderOffset(float x, float y)
        {
            if (_player.Collider.GetType() != typeof(CapsuleCollider2D))
                return;

            Vector2 vec = new Vector2(x, y);
            _player.Collider.offset = vec;
        }
        public void SetColliderSize(float x, float y)
        {
            if (_player.Collider.GetType() != typeof(CapsuleCollider2D))
                return;

            Vector2 vec = new Vector2(x, y);
            CapsuleCollider2D coll = (CapsuleCollider2D)_player.Collider;
            coll.size = vec;
        }
        #endregion

        void FixedUpdate()
        {
            CheckGrounded();
            CheckTouchingWall();
            GetClosestBall();
            MarkClosestBall();
        }

        #region Ground & Wall Check
        void CheckGrounded()
        {
            IsGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _whatIsGround);
            _player.Animator.SetBool("Grounded", IsGrounded);
        }
        void CheckTouchingWall()
        {
            IsTouchingWall = Physics2D.OverlapBox(_wallCheck.position, _wallCheckSize, 0f, _whatIsWall);
            _player.Animator.SetBool("TouchingWall", IsTouchingWall);
        }
        #endregion

        #region Closest Ball
        void GetClosestBall()
        {
            var hitArray = Physics2D.CircleCastAll(_grabJoint.position, 0.8f, Vector3.forward, 0f, _closestLayerMask/*LayerMask.GetMask("BallPassive")*/);

            if (_closestColliders.Count > 0)
                _closestColliders.Clear();

            if (hitArray.Length > 0)
            {
                foreach (var hit in hitArray)
                {
                    if (hit.collider.gameObject.layer.Equals(LayerMask.NameToLayer("BallPassive")))
                    {
                        _closestColliders.Add(hit.collider);
                    }
                }
            }

            if (_closestColliders.Count > 0)
            {
                foreach (var item in _closestColliders)
                {
                    var distance = Vector2.Distance(_grabJoint.position, item.transform.position);
                    if (distance < _closestDistance)
                    {
                        _closest = item.transform;
                        _closestDistance = distance;
                    }
                }
            }
            else
            {
                if (_closest != null)
                    _closest = null;
            }

            _closestDistance = 99999f;
        }
        void MarkClosestBall()
        {
            if (_selectArrow == null)
                return;

            if (_closest != null)
            {
                _selectArrow.gameObject.SetActive(true);
                _selectArrow.SetTarget(_closest);
            }
            else
            {
                _selectArrow.SetTarget(null);
                _selectArrow.gameObject.SetActive(false);
            }
        }
        #endregion

        void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
            Gizmos.DrawWireCube(_wallCheck.position, _wallCheckSize);
        }
    }
}