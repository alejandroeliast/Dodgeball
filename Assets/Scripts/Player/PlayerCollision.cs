using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerCollision : MonoBehaviour
    {
        [SerializeField] Player _player;

        [SerializeField] float groundCheckRadius;
        [SerializeField] Transform groundCheck;
        [SerializeField] LayerMask whatIsGround;

        [SerializeField] Transform _grabJoint;
        [SerializeField] ArrowFollow _selectArrow;

        [SerializeField] LayerMask _closestLayerMask;
        HashSet<Collider2D> _closestColliders = new HashSet<Collider2D>();
        float _closestDistance = 99999f;
        Transform _closest = null;
        public Transform Closest => _closest;


        public bool IsGrounded { get; private set; }
        private void Start()
        {
            _player = GetComponent<Player>();
            _closestLayerMask = LayerMask.GetMask("BallPassive");
        }

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

        private void FixedUpdate()
        {
            CheckSurroundings();
            GetClosestBall();
            MarkClosestBall();
        }

        private void CheckSurroundings()
        {
            IsGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
            _player.Animator.SetBool("Grounded", IsGrounded);
        }

        private void GetClosestBall()
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

        private void MarkClosestBall()
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


        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}