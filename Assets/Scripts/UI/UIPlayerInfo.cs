using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerInfo : MonoBehaviour
{
    [SerializeField] int _playerIndex = 1;

    [SerializeField] List<Image> _healthSlots = new List<Image>();
    [SerializeField] List<Image> _ballSlots = new List<Image>();

    public int PlayerIndex => _playerIndex;

    private void Start()
    {
        ResetInventory();
    }

    public void UpdateInventory(List<string> balls)
    {
        if (balls.Count <= 0)        
            ResetInventory();        
        else
        {
            ResetInventory();
            _ballSlots[0].enabled = true;

            for (int i = 0; i < balls.Count; i++)
            {
                _ballSlots[i].sprite = Resources.Load<Sprite>($"Sprites/{balls[i]}");
                
            }
        }
    }

    private void ResetInventory()
    {
        _ballSlots[0].sprite = Resources.Load<Sprite>($"UI/Match/P{_playerIndex}/P{_playerIndex} Cross Main");

        _ballSlots[0].enabled = false;

        _ballSlots[1].sprite = Resources.Load<Sprite>($"UI/Match/P{_playerIndex}/P{_playerIndex} Cross Side");
        _ballSlots[2].sprite = Resources.Load<Sprite>($"UI/Match/P{_playerIndex}/P{_playerIndex} Cross Side");
    }
}
