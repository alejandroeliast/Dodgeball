using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayerInfoManager : MonoBehaviour
{
    public UIPlayerInfo P1Info { get; private set; }
    public UIPlayerInfo P2Info { get; private set; }
    UIPlayerInfo _p3Info;
    UIPlayerInfo _p4Info;
    private void Start()
    {
        foreach (UIPlayerInfo info in GetComponentsInChildren<UIPlayerInfo>())
        {
            switch (info.PlayerIndex)
            {
                case 1:
                    P1Info = info;
                    break;
                case 2:
                    P2Info = info;
                    break;
                case 3:
                    _p3Info = info;
                    break;
                case 4:
                    _p4Info = info;
                    break;
                default:
                    break;
            }
        }
    }

    private void OnEnable()
    {
        Player.PlayerAction.OnBallThrown += UpdateUI;
        Player.PlayerAction.OnBallGrabbed += UpdateUI;
    }
    private void OnDisable()
    {
        Player.PlayerAction.OnBallThrown -= UpdateUI;
        Player.PlayerAction.OnBallGrabbed -= UpdateUI;
    }

    private void UpdateUI(int index, List<string> balls)
    {
        switch (index)
        {
            case 1:
                P1Info.UpdateInventory(balls);
                break;
            case 2:
                P2Info.UpdateInventory(balls);
                break;
            case 3:
                _p3Info.UpdateInventory(balls);
                break;
            case 4:
                _p4Info.UpdateInventory(balls);
                break;
            default:
                break;
        }
    }
}
