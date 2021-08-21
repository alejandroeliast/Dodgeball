using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    int numberOfPlayers;
    int playersAlive;

    int player1Score;
    int player2Score;
    int player3Score;
    int player4Score;
    private RoundManager roundManager;

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

    public void OnPlayerDeath(Character attacked, Character attacker)
    {
        playersAlive--;

        if (playersAlive == 1)
            AddScore(attacker);
        else if (playersAlive <= 0)
            Debug.Log("Draw");

        if (playersAlive <= 1)
        {
            roundManager.RoundWon(attacker);
        }
    }

    void AddScore(Character character)
    {
        switch (character.Index)
        {
            case 1:
                player1Score++;
                break;
            case 2:
                player2Score++;
                break;
            case 3:
                player3Score++;
                break;
            case 4:
                player4Score++;
                break;
            default:
                break;
        }


    }

    public bool CheckIfMatchHasBeenWon()
    {
        if (player1Score >= 3 || player2Score >= 3 || player3Score >= 3 || player4Score >= 3)
            return true;

        return false;
    }

    public Player CheckWhoWonMatch()
    {
        if (player1Score >= 3)
            return PlayerManager.Instance.players[0];
        else if (player2Score >= 3)
            return PlayerManager.Instance.players[1];
        else if (player3Score >= 3)
            return PlayerManager.Instance.players[2];
        else if (player4Score >= 3)
            return PlayerManager.Instance.players[3];

        return null;
    }

    public void ResetScores()
    {
        player1Score = 0;
        player2Score = 0;
        player3Score = 0;
        player4Score = 0;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        numberOfPlayers = PlayerManager.Instance.players.Count;
        playersAlive = numberOfPlayers;
        print(player1Score);

        roundManager = FindObjectOfType<RoundManager>();
    }
}
