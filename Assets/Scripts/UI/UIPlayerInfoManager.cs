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
        OldPlayer.PlayerAction.OnTakeDamage += UpdateHealth;

        OldPlayer.PlayerAction.OnBallThrown += UpdateInventory;
        OldPlayer.PlayerAction.OnBallGrabbed += UpdateInventory;
    }
    private void OnDisable()
    {
        OldPlayer.PlayerAction.OnTakeDamage -= UpdateHealth;

        OldPlayer.PlayerAction.OnBallThrown -= UpdateInventory;
        OldPlayer.PlayerAction.OnBallGrabbed -= UpdateInventory;
    }

    private void UpdateHealth(int index, int health)
    {
        if (_playerInfoList[index] == null)
            return;

        _playerInfoList[index].UpdateHealth(health);
    }

    private void UpdateInventory(int index, List<BallSO> balls)
    {
        if (_playerInfoList[index] == null)
            return;

        _playerInfoList[index].UpdateInventory(balls);
    }
}
