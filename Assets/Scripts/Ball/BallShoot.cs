using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallShoot : MonoBehaviour, IBallBehaviour
{
    new Rigidbody2D rigidbody2D;

    public BallController Controller { get; set; }

    public void Initialize(BallController controller)
    {
        Controller = controller;
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void Shoot(Vector2 direction, float charge)
    {
        var speed = charge * Controller.BallSO.ballMaxSpeed;
        if (speed < Controller.BallSO.ballMinSpeed)
            speed = Controller.BallSO.ballMinSpeed;

        rigidbody2D.velocity = direction * speed;

        rigidbody2D.gravityScale = 0;
        rigidbody2D.AddRelativeForce(direction, ForceMode2D.Impulse);

        Controller.Follow.SetTarget(null);
    }
}
