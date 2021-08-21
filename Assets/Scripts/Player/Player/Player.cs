using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Character characterPrefab;

    public event Action<Character> OnCharacterChanged = delegate { };

    public bool HasController => Controller != null;
    public int PlayerNumber { get; private set; }
    public Controller Controller { get; private set; }
    public Character CharacterPrefab { get; set; }

    public void InitializePlayer(Controller controller)
    {
        Controller = controller;
        PlayerNumber = controller.Index;
        gameObject.name = string.Format("Player {0} - {1}", PlayerNumber, controller.gameObject.name);

        SpawnCharacter();
    }
    public void SpawnCharacter()
    {
        Vector3 spawnPosition = GetSpawnPosition();

        var character = Instantiate(characterPrefab, spawnPosition, Quaternion.identity);
        character.SetController(Controller);
        character.Index = PlayerNumber;
        character.Health = 3;

        Controller.character = character;

        OnCharacterChanged(character);
    }

    private Vector3 GetSpawnPosition()
    {
        var spawnPosition = Vector3.zero;

        var spawnPointManager = FindObjectOfType<SpawnPointManager>();
        if (spawnPointManager != null)
        {
            foreach (var spawnPoint in spawnPointManager.SpawnPoints)
            {
                if (spawnPoint.Index == PlayerNumber)
                {
                    spawnPosition = spawnPoint.transform.position;
                    break;
                }
            }
        }

        return spawnPosition;
    }
}
