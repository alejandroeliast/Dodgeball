using System;
using System.Collections;
using UnityEngine;

public class ReadyTrigger : MonoBehaviour
{
    int playersReady = 0;
    Coroutine countdown;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var character = collision.GetComponent<Character>();
        if (character != null)
            playersReady++;

        if (playersReady < PlayerManager.Instance.players.Count)
            return;

        print("All ready");
        countdown = StartCoroutine(LoadArena());
    }

    private IEnumerator LoadArena()
    {
        print("5");
        yield return new WaitForSeconds(1f);
        print("4");
        yield return new WaitForSeconds(1f);
        print("3");
        yield return new WaitForSeconds(1f);
        print("2");
        yield return new WaitForSeconds(1f);
        print("1");
        yield return new WaitForSeconds(1f);
        print("Loading Arena");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var character = collision.GetComponent<Character>();
        if (character != null)        
            playersReady--;

        StopCoroutine(countdown);
        print("Not all players ready");
    }
}
