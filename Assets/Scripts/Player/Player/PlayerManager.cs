using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    //[SerializeField] List<Transform> _startingPositions = new List<Transform>();
    //[SerializeField] List<GameObject> _playerList = new List<GameObject>();

    //[SerializeField] UIPlayerInfoManager _playerInfoManager;

    //[SerializeField] List<TextMeshProUGUI> _texts = new List<TextMeshProUGUI>();
    //[SerializeField] Image _background;

    bool _isLobbyFull;
    [SerializeField] Player playerPrefab;

    public List<Player> players = new List<Player>();

    public static PlayerManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;

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

    public void OnPlayerJoin()
    {
        #region Old
        //var temp = GameObject.FindGameObjectsWithTag("Player");

        //foreach (var item in temp)
        //{
        //    if (!_playerList.Contains(item))
        //    {
        //        _playerList.Add(item);
        //    }
        //}

        //for (int i = 0; i < _playerList.Count; i++)
        //{
        //    _texts[i].text = $"Player {i + 1} is Ready!";

        //    var player = _playerList[i].GetComponent<Player.Player>();

        //    player.Index = i;
        //    player.Health = 3;
        //    player._playerUI = _playerInfoManager.PlayerInfoList[i];
        //}

        //_isLobbyFull = _playerList.Count == 2;

        //if (_isLobbyFull)
        //{

        //    for (int i = 0; i < _playerList.Count; i++)
        //    {
        //        _playerList[i].transform.position = _startingPositions[i].position;
        //    }


        //    foreach (var item in _texts)
        //    {
        //        item.enabled = false;
        //    }
        //    _background.enabled = false;
        //}
        #endregion
    }

    internal void AddPlayerToGame(Controller controller)
    {
        var player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        players.Add(player);
        player.transform.parent = transform;
        player.InitializePlayer(controller);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (var player in players)
        {
            player.SpawnCharacter();
        }
    }
}