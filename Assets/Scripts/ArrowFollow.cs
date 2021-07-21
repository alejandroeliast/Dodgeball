using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFollow : MonoBehaviour
{
    [SerializeField] Transform _target;

    SpriteRenderer _spriteRenderer;
    Floating _floating;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (_target == null)
            return;

        FollowBall();
    }

    public void FollowBall()
    {
        transform.position = _target.position + new Vector3(0f, 0.3f, 0f);
        _spriteRenderer.enabled = true;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
}
