using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDispenser : MonoBehaviour
{
    [SerializeField] BallSO _ballSO;
    [SerializeField] Transform _displayJoint;

    GameObject _defaultPrefab;
    GameObject _ballDisplayed;

    private void Start()
    {
        _defaultPrefab = Resources.Load<GameObject>("Prefabs/Ball Display");
        SpawnBall();
    }

    private void SpawnBall()
    {
        if (_ballDisplayed != null)
            return;

        _ballDisplayed = Instantiate(_defaultPrefab, _displayJoint.position, Quaternion.identity);
        var controller = _ballDisplayed.GetComponent<BallController>();

        if (controller != null)
            controller.BallSetUp(_ballSO);
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
