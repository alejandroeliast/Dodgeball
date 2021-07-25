using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] BallType _ballType;

    bool isFollowing = true;
    int _bouncesRemaining = 3;

    Rigidbody2D _rigidbody2D;
    Collider2D _collider2D;
    GameObject _parent;

    public BallType Type => _ballType;

    public enum BallType
    {
        Base,
        Big
    }

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (_parent == null)
            return;

        if (isFollowing)
        {
            transform.position = _parent.transform.position;
        }
    }
    private void FixedUpdate()
    {
        if (_parent == null)
            return;

        if (this.gameObject.layer.Equals(LayerMask.NameToLayer("BallActive")) && !Physics2D.GetIgnoreCollision(_parent.GetComponentInParent<Collider2D>(), _collider2D))
        {
            Physics2D.IgnoreCollision(_parent.GetComponentInParent<Collider2D>(), _collider2D, true);
        }
    }

    public void HoldStart(GameObject parent)
    {
        this.gameObject.layer = LayerMask.NameToLayer("BallHeld");

        _parent = parent;
    }


    public void HoldEnd()
    {
        this.gameObject.layer = LayerMask.NameToLayer("BallActive");
    }
    public void StopFollowPlayer()
    {
        isFollowing = false;
    }

    public void Shoot(Vector2 direction, float speed)
    {
        _rigidbody2D.velocity = direction * speed;

        _rigidbody2D.gravityScale = 0;
        _rigidbody2D.AddRelativeForce(direction, ForceMode2D.Impulse);
        //StartCoroutine(EnableGravity());

        StopFollowPlayer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _bouncesRemaining--;
        var vel = _rigidbody2D.velocity;
        if (_bouncesRemaining <= 0)
        {
            _rigidbody2D.velocity *= 0.5f;
            _rigidbody2D.gravityScale = 1;

            gameObject.layer = LayerMask.NameToLayer("BallPassive");
            gameObject.tag = "Ball";
            //StartCoroutine(EnableGravity());
        }
        else
        {
            _rigidbody2D.velocity = vel;
        }

        if(collision.collider.gameObject.CompareTag("Player") && collision.collider != _parent.GetComponentInParent<Collider2D>())
        {
            _rigidbody2D.velocity *= 0.5f;
            _rigidbody2D.gravityScale = 1;

            gameObject.layer = LayerMask.NameToLayer("BallPassive");
            gameObject.tag = "Ball";
            print("Player hit");
        }

        //if (_parent == null)
        //    return;

        //if (_parent.GetComponentInParent<Collider2D>() == collision.collider)
        //    return;

        //var player = collision.collider.GetComponent<Player.PlayerController>();
        //if (player == null)
        //    return;

        //print("Hit player");



    }

    IEnumerator EnableGravity()
    {
        yield return new WaitForSeconds(0.5f);
        _rigidbody2D.gravityScale = 1;
    }
}
