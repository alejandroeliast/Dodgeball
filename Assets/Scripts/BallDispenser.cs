using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDispenser : MonoBehaviour
{
    [SerializeField] GameObject _ballPrefab;
    [SerializeField] Transform _displayJoint;

    GameObject _ballDisplayed;

    private void Start()
    {
        SpawnBall();
    }

    private void SpawnBall()
    {
        if (_ballDisplayed != null)
            return;

        _ballDisplayed = Instantiate(_ballPrefab, _displayJoint.position, Quaternion.identity);
    }

    private void Update()
    {
        CheckForBall();
    }

    private void CheckForBall()
    {
        if (_ballDisplayed != null)
            return;

        StartCoroutine(BallRespawn());
    }

    private IEnumerator BallRespawn()
    {
        yield return new WaitForSeconds(1f);
        SpawnBall();
    }
}
