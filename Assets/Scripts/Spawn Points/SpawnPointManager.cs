using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

    public List<SpawnPoint> SpawnPoints => spawnPoints;

    private void Awake()
    {
        spawnPoints = GetComponentsInChildren<SpawnPoint>().ToList();
        spawnPoints.OrderBy(t => t.Index);
    }
}
