using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHold : MonoBehaviour, IBallBehaviour
{
    new Collider2D collider2D;
    public BallController Controller { get; set; }

    public void Initialize(BallController controller)
    {
        Controller = controller;
        collider2D = GetComponent<Collider2D>();
    }

    public void HoldStart(Transform parent)
    {
        this.gameObject.layer = LayerMask.NameToLayer("BallHeld");
        Controller.Follow.SetTarget(parent);

        if (collider2D != null)
            collider2D.enabled = false;
    }

    public void HoldEnd()
    {
        this.gameObject.layer = LayerMask.NameToLayer("BallActive");

        if (collider2D != null)
            collider2D.enabled = true;
    }
}
