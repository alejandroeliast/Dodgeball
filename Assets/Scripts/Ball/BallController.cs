using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] BallType _ballType;

    bool _isFollowing = true;
    int _bouncesRemaining = 3;

    BallSO _ballSO;

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

    public void BallSetUp(BallSO ballSO)
    {
        _ballSO = ballSO;

        GetComponent<SpriteRenderer>().sprite = _ballSO.ballGameSprite;
        gameObject.name = _ballSO.ballName;

        _bouncesRemaining = _ballSO.ballMaxBounces;
    }

    private void Update()
    {
        if (ShouldFollowParent())
            FollowParent();
    }
    #region Follow Parent
    void FollowParent()
    {
        transform.position = _parent.transform.position;
    }
    bool ShouldFollowParent()
    {
        if (_parent == null)
            return false;

        if (!_isFollowing)
            return false;

        return true;
    }
    #endregion

    private void FixedUpdate()
    {
        if (ShouldIgnoreParentCollision())
            IgnoreParentCollision();
    }
    #region Ignore Parent Collision
    private void IgnoreParentCollision()
    {
        Physics2D.IgnoreCollision(_parent.GetComponentInParent<Collider2D>(), _collider2D, true);
    }
    bool ShouldIgnoreParentCollision()
    {
        if (_parent == null)
            return false;

        if (!gameObject.layer.Equals(LayerMask.NameToLayer("BallActive")))
            return false;

        if (Physics2D.GetIgnoreCollision(_parent.GetComponentInParent<Collider2D>(), _collider2D))
            return false;

        return true;
    }
    #endregion

    #region Holding
    public void HoldStart(GameObject parent)
    {
        this.gameObject.layer = LayerMask.NameToLayer("BallHeld");
        _parent = parent;

        if (_collider2D != null)
            _collider2D.enabled = false;
    }
    public void HoldEnd()
    {
        this.gameObject.layer = LayerMask.NameToLayer("BallActive");

        if (_collider2D != null)
            _collider2D.enabled = true;
    }
    #endregion

    public void Shoot(Vector2 direction, float charge)
    {
        var speed = charge * _ballSO.ballMaxSpeed;
        if (speed < _ballSO.ballMinSpeed)
            speed = _ballSO.ballMinSpeed;

        _rigidbody2D.velocity = direction * speed;

        _rigidbody2D.gravityScale = 0;
        _rigidbody2D.AddRelativeForce(direction, ForceMode2D.Impulse);
        //StartCoroutine(EnableGravity());

        _isFollowing = false;
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

        if (collision.collider.gameObject.CompareTag("Player") && collision.collider != _parent.GetComponentInParent<Collider2D>())
        {
            _rigidbody2D.velocity *= 0.5f;
            _rigidbody2D.gravityScale = 1;

            collision.collider.GetComponent<Player.Player>().Action.TakeDamage();
            Destroy(gameObject);

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
