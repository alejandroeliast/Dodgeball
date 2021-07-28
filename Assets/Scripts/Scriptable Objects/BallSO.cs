using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Ball", menuName = "Assets/Resources/New Ball")]
public class BallSO : ScriptableObject
{
    public enum BallSize
    {
        Default,
        Big
    }

    [Header("About")]
    public int ballID;
    public string ballName;
    public GameObject ballPrefab;

    [Header("Sprites")]
    public Sprite ballGameSprite;
    public Sprite ballUISprite;

    [Header("Specifications")]
    public BallSize ballSize;
    public float ballMinSpeed;
    public float ballMaxSpeed;
    public int ballMaxBounces;
}
