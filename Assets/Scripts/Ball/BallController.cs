using UnityEngine;

public class BallController : MonoBehaviour
{
    public BallSO BallSO { get; private set; }
    public Character Owner { get; private set; }
    public BallFollow Follow { get; private set; }
    public BallHold Hold { get; private set; }
    public BallCollision Collision { get; private set; }
    public BallShoot Shoot { get; private set; }
    public int BouncesRemaining { get; set; }

    void Awake()
    {
        Follow = GetComponent<BallFollow>();
        Hold = GetComponent<BallHold>();
        Collision = GetComponent<BallCollision>();
        Shoot = GetComponent<BallShoot>();

        foreach (var behaviour in GetComponents<IBallBehaviour>())
        {
            behaviour?.Initialize(this);
        }
    }

    public void BallSetUp(BallSO ballSO, Character character = null)
    {
        BallSO = ballSO;

        GetComponent<SpriteRenderer>().sprite = BallSO.ballGameSprite;
        gameObject.name = BallSO.ballName;
        Owner = character;

        BouncesRemaining = BallSO.ballMaxBounces;
    }
}
