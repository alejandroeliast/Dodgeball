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

    void Start()
    {
        ResetHealth();
        ResetInventory();
    }

    #region Health
    public void UpdateHealth(int health)
    {
        EmptyHealth();

        if (health > 0)
        {
            for (int i = 0; i < health; i++)
            {
                var anchoredPos = _healthSlots[i].GetComponent<RectTransform>().anchoredPosition;
                anchoredPos = new Vector2(anchoredPos.x, -50f);

                _healthSlots[i].sprite = Resources.Load<Sprite>($"UI/Match/P{_playerIndex}/P{_playerIndex} Health Full");
            }
        }
    }
    void EmptyHealth()
    {
        Sprite healthEmpty = Resources.Load<Sprite>($"UI/Match/P{_playerIndex}/P{_playerIndex} Health Empty");

        foreach (Image item in _healthSlots)
        {
            item.sprite = healthEmpty;

            var anchoredPos = item.GetComponent<RectTransform>().anchoredPosition;
            anchoredPos = new Vector2(anchoredPos.x, -15f);
        }
    }
    void ResetHealth()
    {
        Sprite healthFull = Resources.Load<Sprite>($"UI/Match/P{_playerIndex}/P{_playerIndex} Health Full");

        foreach (Image item in _healthSlots)
        {
            item.sprite = healthFull;

            var anchoredPos = item.GetComponent<RectTransform>().anchoredPosition;
            anchoredPos = new Vector2(anchoredPos.x, -25f);
        }
    }
    #endregion

    #region Inventory
    public void UpdateInventory(List<string> balls)
    {
        ResetInventory();

        if (balls.Count > 0)
        {
            _ballSlots[0].enabled = true;

            for (int i = 0; i < balls.Count; i++)
                _ballSlots[i].sprite = Resources.Load<Sprite>($"Sprites/{balls[i]}");
        }
    }
    private void ResetInventory()
    {
        _ballSlots[0].sprite = Resources.Load<Sprite>($"UI/Match/P{_playerIndex}/P{_playerIndex} Cross Main");
        _ballSlots[0].enabled = false;

        for (int i = 1; i <= 2; i++)
            _ballSlots[i].sprite = Resources.Load<Sprite>($"UI/Match/P{_playerIndex}/P{_playerIndex} Cross Side");
    }
    #endregion
}

