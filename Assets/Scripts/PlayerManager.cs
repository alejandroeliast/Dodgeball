using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] List<Transform> _startingPositions = new List<Transform>();
    [SerializeField] List<GameObject> _playerList = new List<GameObject>();

    private void Start()
    {
        _playerList.Clear();
    }

    public void OnPlayerJoin()
    {
        var temp = GameObject.FindGameObjectsWithTag("Player");

        foreach (var item in temp)
        {
            if (!_playerList.Contains(item))
            {
                _playerList.Add(item);
            }
        }

        for (int i = 0; i < _playerList.Count; i++)
        {
            _playerList[i].transform.position = _startingPositions[i].position;
        }
    }
}
