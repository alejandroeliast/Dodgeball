using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIPlayerInfoManager : MonoBehaviour
{
    List<UIPlayerInfo> _playerInfoList = new List<UIPlayerInfo>();
    public List<UIPlayerInfo> PlayerInfoList => _playerInfoList;

    private void Start()
    {
        foreach (UIPlayerInfo info in GetComponentsInChildren<UIPlayerInfo>())
        {
            if (!_playerInfoList.Contains(info))
                _playerInfoList.Add(info);
        }

        var temp = _playerInfoList.OrderBy(e => e.PlayerIndex).ToList();
        _playerInfoList.Clear();
        _playerInfoList = temp;
    }

    private void OnEnable()
    {
        CharacterAction.OnTakeDamage += UpdateHealth;
        
        CharacterAction.OnBallThrown += UpdateInventory;
        CharacterAction.OnBallGrabbed += UpdateInventory;
    }
    private void OnDisable()
    {
        CharacterAction.OnTakeDamage -= UpdateHealth;

        CharacterAction.OnBallThrown -= UpdateInventory;
        CharacterAction.OnBallGrabbed -= UpdateInventory;
    }

    private void UpdateHealth(int index, int health)
    {
        if (_playerInfoList[index - 1] == null)
            return;

        _playerInfoList[index - 1].UpdateHealth(health);
    }

    private void UpdateInventory(int index, List<BallSO> balls)
    {
        if (_playerInfoList[index - 1] == null)
            return;

        _playerInfoList[index - 1].UpdateInventory(balls);
    }
}
