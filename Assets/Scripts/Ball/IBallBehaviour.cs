using UnityEngine;

public interface IBallBehaviour
{
    public BallController Controller { get; set; }
    public void Initialize(BallController controller);
}
