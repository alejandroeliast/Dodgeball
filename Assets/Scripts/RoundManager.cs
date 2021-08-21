using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    int numberOfRounds;
    private ScoreManager scoreManager;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    public void SetRoundNumber(int amount)
    {
        numberOfRounds = amount;
    }

    public void RoundEnd()
    {

    }

    internal void RoundWon(Character winner)
    {
        foreach (var controller in ControllerManager.Instance.Controllers)
        {
            controller.SetEnabled();
        }

        if (scoreManager.CheckIfMatchHasBeenWon())
        {
            MatchEnd(scoreManager.CheckWhoWonMatch());
            return;
        }

        SceneManager.LoadScene(2);
    }

    internal void MatchEnd(Player winner)
    {
        print(winner + " has won");

        scoreManager.ResetScores();
        SceneManager.LoadScene(1);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }
}
