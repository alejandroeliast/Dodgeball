using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] List<Transform> _startingPositions = new List<Transform>();
    [SerializeField] List<GameObject> _playerList = new List<GameObject>();

    [SerializeField] UIPlayerInfoManager _playerInfoManager;

    [SerializeField] List<TextMeshProUGUI> _texts = new List<TextMeshProUGUI>();
    [SerializeField] Image _background;

    bool _isLobbyFull;

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
            _texts[i].text = $"Player {i + 1} is Ready!";
            _playerList[i].GetComponent<Player.Player>().Index = i + 1;


            if (i == 0)
                _playerList[i].GetComponent<Player.Player>()._playerUI = _playerInfoManager.P1Info;
            else if (i == 1)
                _playerList[i].GetComponent<Player.Player>()._playerUI = _playerInfoManager.P2Info;

        }

        _isLobbyFull = _playerList.Count == 2;

        if (_isLobbyFull)
        {

            for (int i = 0; i < _playerList.Count; i++)
            {
                _playerList[i].transform.position = _startingPositions[i].position;
            }


            foreach (var item in _texts)
            {
                item.enabled = false;
            }
            _background.enabled = false;
        }
    }
}
