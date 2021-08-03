using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollision : MonoBehaviour
{
    #region Variables
    // Main Player Script reference
    Character character;

    // Ground Check
    [Header("Ground Check")]
    [SerializeField] float groundCheckRadius;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask whatIsGround;
    public bool IsGrounded { get; private set; }

    // Wall Check
    [Header("Wall Check")]
    [SerializeField] Vector2 wallCheckSize;
    [SerializeField] Transform wallCheck;
    [SerializeField] LayerMask whatIsWall;
    public bool IsTouchingWall { get; private set; }

    // Grab Check
    [Header("Grab Check")]
    [SerializeField] Transform grabJoint;
    [SerializeField] ArrowFollow selectArrow;
    [SerializeField] LayerMask closestLayerMask;

    float closestDistance = 99999f;
    Transform closest = null;
    HashSet<Collider2D> closestColliders = new HashSet<Collider2D>();

    public Transform Closest => closest;
    #endregion

    void Start()
    {
        closestLayerMask = LayerMask.GetMask("BallPassive");
    }

    public void Initialize(Character character)
    {
        this.character = character;
    }

    #region Set Collider Parameters
    public void SetColliderOffset(float x, float y)
    {
        if (character.Collider.GetType() != typeof(CapsuleCollider2D))
            return;

        Vector2 vec = new Vector2(x, y);
        character.Collider.offset = vec;
    }
    public void SetColliderSize(float x, float y)
    {
        if (character.Collider.GetType() != typeof(CapsuleCollider2D))
            return;

        Vector2 vec = new Vector2(x, y);
        CapsuleCollider2D coll = (CapsuleCollider2D)character.Collider;
        coll.size = vec;
    }
    #endregion

    void FixedUpdate()
    {
        CheckGrounded();
        CheckTouchingWall();
        GetClosestBall();
        MarkClosestBall();
    }

    #region Ground & Wall Check
    void CheckGrounded()
    {
        IsGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        character.Animator.SetBool("Grounded", IsGrounded);
    }
    void CheckTouchingWall()
    {
        IsTouchingWall = Physics2D.OverlapBox(wallCheck.position, wallCheckSize, 0f, whatIsWall);
        character.Animator.SetBool("TouchingWall", IsTouchingWall);
    }
    #endregion

    #region Closest Ball
    void GetClosestBall()
    {
        var hitArray = Physics2D.CircleCastAll(grabJoint.position, 0.8f, Vector3.forward, 0f, closestLayerMask/*LayerMask.GetMask("BallPassive")*/);

        if (closestColliders.Count > 0)
            closestColliders.Clear();

        GetAllPassiveBalls(hitArray);

        FindShortestDistance();

        closestDistance = 99999f;
    }

    private void FindShortestDistance()
    {
        if (closestColliders.Count > 0)
        {
            foreach (var item in closestColliders)
            {
                var distance = Vector2.Distance(grabJoint.position, item.transform.position);
                if (distance < closestDistance)
                {
                    closest = item.transform;
                    closestDistance = distance;
                }
            }
        }
        else
        {
            if (closest != null)
                closest = null;
        }
    }

    private void GetAllPassiveBalls(RaycastHit2D[] hitArray)
    {
        if (hitArray.Length > 0)
        {
            foreach (var hit in hitArray)
            {
                if (hit.collider.gameObject.layer.Equals(LayerMask.NameToLayer("BallPassive")))
                {
                    closestColliders.Add(hit.collider);
                }
            }
        }
    }

    void MarkClosestBall()
    {
        if (selectArrow == null)
            return;

        if (closest != null)
        {
            selectArrow.gameObject.SetActive(true);
            selectArrow.SetTarget(closest);
        }
        else
        {
            selectArrow.SetTarget(null);
            selectArrow.gameObject.SetActive(false);
        }
    }
    #endregion

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
    }
}
