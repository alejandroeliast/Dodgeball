using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] int index;

    public int Index => index;

    private void Start()
    {
        gameObject.name = "Spawn Point " + index;
    }
}
