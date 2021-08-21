using UnityEngine;

public class BallFollow : MonoBehaviour
{
    Transform target;

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    void Update()
    {
        if (ShouldFollowTarget())
            FollowTarget();
    }

    bool ShouldFollowTarget()
    {
        if (target == null)
            return false;

        return true;
    }

    void FollowTarget()
    {
        transform.position = target.position;
    }
}