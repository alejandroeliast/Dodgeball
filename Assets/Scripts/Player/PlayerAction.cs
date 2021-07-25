using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerAction : MonoBehaviour
    {
        [SerializeField] Player _player;

        [SerializeField] BallController _ballPrefab;
        [SerializeField] GameObject _handJoint;
        [SerializeField] GameObject _launchJoint;
        [SerializeField] float _ballForce;
        GameObject _childBall;

        [SerializeField] GameObject _reticle;

        bool _isThrowing;
        float _throwTimer = 0f;
        Vector2 _aimVector;

        private void Start()
        {
            _player = GetComponent<Player>();
        }

        public void OnAimChanged(Vector2 aimVector)
        {
            _aimVector = aimVector;
        }


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
            if (_player.ballList.Count <= 0)
                return;

            if (_childBall != null)
                return;

            var randomAngle = UnityEngine.Random.Range(0, 180);
            var toSpawn = Resources.Load($"Prefabs/{_player.ballList[0]}");

            _childBall = Instantiate((GameObject)toSpawn, _handJoint.transform.position, Quaternion.Euler(0, 0, randomAngle));

            var controller = _childBall.GetComponent<BallController>();
            controller.HoldStart(_handJoint);

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

                Vector2 direction = cursorInWorldPos - (Vector2)transform.position;
                direction.Normalize();

                controller.Shoot(direction, _ballForce * charge);
            }
            else
            {
                _aimVector.Normalize();
                controller.Shoot(_aimVector, _ballForce * charge);
            }

            controller.HoldEnd();

            _childBall.transform.parent = null;
            _childBall = null;

            _player.ballList.Remove(_player.ballList[0]);

            _reticle.SetActive(false);
            _player.Animator.SetBool("Throw", false);

            _player.UpdateUIList();
        }

        public void Grab()
        {
            if (_player.Collision.Closest == null)
                return;

            if (_player.ballList.Count >= 3)
                return;

            var controller = _player.Collision.Closest.gameObject.GetComponent<BallController>();

            _player.ballList.Add(controller.Type.ToString());
            Destroy(_player.Collision.Closest.gameObject);

            _player.UpdateUIList();
        }

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
                var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
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