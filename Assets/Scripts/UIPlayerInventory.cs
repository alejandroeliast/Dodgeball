using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerInventory : MonoBehaviour
{
    public List<Image> _p1Images = new List<Image>();

    private void Start()
    {
        foreach (var item in _p1Images)
        {
            item.sprite = Resources.Load<Sprite>($"Sprites/Peg"); ;
        }
    }

    public void UpdateList(List<string> list)
    {
        foreach (var item in _p1Images)
        {
            item.sprite = Resources.Load<Sprite>($"Sprites/Peg");
        }

        for (int i = 0; i < list.Count; i++)
        {
            _p1Images[i].sprite = Resources.Load<Sprite>($"Sprites/{list[i]}");
            _p1Images[i].SetNativeSize();
        }
    }
}
