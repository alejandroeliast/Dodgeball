using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerAction : MonoBehaviour
    {
        // References
        [SerializeField] Player _player;

        [SerializeField] BallController _ballPrefab;
        [SerializeField] GameObject _handJoint;
        [SerializeField] GameObject _launchJoint;

        GameObject _defaultPrefab;
        GameObject _childBall;

        [SerializeField] GameObject _reticle;

        bool _isThrowing;
        float _throwTimer = 0f;
        Vector2 _aimVector;

        public List<BallSO> _ballList = new List<BallSO>();

        public static event Action<int, List<BallSO>> OnBallGrabbed;
        public static event Action<int, List<BallSO>> OnBallThrown;
        public static event Action<int, int> OnTakeDamage;

        public bool IsThrowing => _isThrowing;

        private void Start()
        {
            _player = GetComponent<Player>();
            _defaultPrefab = Resources.Load<GameObject>("Prefabs/Ball Throw");
        }

        public void OnAimChanged(Vector2 aimVector)
        {
            _aimVector = aimVector;
        }

        #region Throw
        public void Throw(bool value)
        {
            _isThrowing = value;

            if (_isThrowing)
                ThrowStart();
            else
                ThrowEnd(Mathf.Clamp(_throwTimer, 0.4f, 1));
        }
        public void ThrowStart()
        {
            if (_ballList.Count <= 0)
                return;

            if (_childBall != null)
                return;

            var randomAngle = UnityEngine.Random.Range(0, 180);

            _childBall = Instantiate(_defaultPrefab, _handJoint.transform.position, Quaternion.Euler(0, 0, randomAngle));

            var controller = _childBall.GetComponent<BallController>();
            controller.HoldStart(_handJoint);
            controller.BallSetUp(_ballList[0]);

            _reticle.SetActive(true);
            _player.Animator.SetBool("Throw", true);
        }
        public void ThrowEnd(float charge)
        {
            if (_childBall == null)
                return;

            var controller = _childBall.GetComponent<BallController>();
            _childBall.transform.position = _launchJoint.transform.position;

            if (_player.Input.ControllerType == "Mouse")
            {
                Vector2 cursorInWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 direction = cursorInWorldPos - (Vector2)_launchJoint.transform.position;
                direction.Normalize();
                controller.Shoot(direction, charge);
            }
            else
            {
                _aimVector.Normalize();
                controller.Shoot(_aimVector, charge);
            }

            controller.HoldEnd();

            _childBall.transform.parent = null;
            _childBall = null;

            _ballList.Remove(_ballList[0]);

            _reticle.SetActive(false);
            _player.Animator.SetBool("Throw", false);

            OnBallThrown?.Invoke(_player.Index, _ballList);
        }
        #endregion

        #region Grab
        public void Grab()
        {
            if (ShouldGrab())
            {
                var ball = Resources.Load<BallSO>($"Scriptable Objects/{_player.Collision.Closest.name}");

                _ballList.Add(ball);
                Destroy(_player.Collision.Closest.gameObject);

                OnBallGrabbed?.Invoke(_player.Index, _ballList);
            }
        }
        bool ShouldGrab()
        {
            if (_player.Collision.Closest == null)
                return false;

            if (_ballList.Count >= 3)
                return false;

            return true;
        }
        #endregion

        #region Damage
        public void TakeDamage()
        {
            _player.Health--;

            OnTakeDamage?.Invoke(_player.Index, _player.Health);

            if (_player.Health <= 0)
            {
                print("Died");
            }
        }
        #endregion

        #region Select Ball
        public void MoveListForward()
        {
            if (_ballList.Count <= 1)
                return;

            BallSO temp = _ballList[0];
            _ballList.Remove(_ballList[0]);
            _ballList.Add(temp);

            OnBallThrown?.Invoke(_player.Index, _ballList);
        }
        public void MoveListBackward()
        {
            if (_ballList.Count <= 1)
                return;

            BallSO temp = _ballList[_ballList.Count - 1];
            _ballList.Remove(_ballList[_ballList.Count - 1]);
            _ballList.Insert(0, temp);

            OnBallThrown?.Invoke(_player.Index, _ballList);
        }
        #endregion

        private void Update()
        {
            RotateReticle();
            if (_isThrowing)
                _throwTimer += Time.deltaTime;
            else
                _throwTimer = 0;
        }

        public void RotateReticle()
        {
            if (_reticle.activeSelf == false)
                return;

            if (_player.Input.ControllerType == "Mouse")
            {
                var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(_launchJoint.transform.position);
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                _reticle.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            }
            else
            {
                var angle = Mathf.Atan2(_aimVector.y, _aimVector.x) * Mathf.Rad2Deg;
                _reticle.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            }
        }
    }
}