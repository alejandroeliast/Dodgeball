using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollision : MonoBehaviour, IBallBehaviour
{
    new Collider2D collider2D;
    new Rigidbody2D rigidbody2D;

    public BallController Controller { get; set; }

    public void Initialize(BallController controller)
    {
        Controller = controller;
        collider2D = GetComponent<Collider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (ShouldIgnoreParentCollision())
            IgnoreParentCollision();
    }

    bool ShouldIgnoreParentCollision()
    {
        if (Controller.Owner == null)
            return false;

        if (!gameObject.layer.Equals(LayerMask.NameToLayer("BallActive")))
            return false;

        if (Physics2D.GetIgnoreCollision(Controller.Owner.GetComponentInParent<Collider2D>(), collider2D))
            return false;

        return true;
    }

    void IgnoreParentCollision()
    {
        Physics2D.IgnoreCollision(Controller.Owner.GetComponentInParent<Collider2D>(), collider2D, true);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Controller.BouncesRemaining--;
        var vel = rigidbody2D.velocity;
        if (Controller.BouncesRemaining <= 0)
        {
            rigidbody2D.velocity *= 0.5f;
            rigidbody2D.gravityScale = 1;

            gameObject.layer = LayerMask.NameToLayer("BallPassive");
            gameObject.tag = "Ball";
            //StartCoroutine(EnableGravity());
        }
        else
        {
            rigidbody2D.velocity = vel;
        }

        if (collision.collider.gameObject.CompareTag("Player") && collision.collider != Controller.Owner.GetComponentInParent<Collider2D>())
        {
            rigidbody2D.velocity *= 0.5f;
            rigidbody2D.gravityScale = 1;

            print(Controller.Owner);
            collision.collider.GetComponent<Character>().Action.TakeDamage(Controller.Owner);
            Destroy(gameObject);

        }
    }

}
