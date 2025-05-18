using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldAIManager : MonoBehaviour
{
    public static WorldAIManager instance;

    [Header("Debug")]
    [SerializeField] bool despawnCharacters = false;
    [SerializeField] bool respawnCharacters = false;

    [Header("AIs")]
    [SerializeField] List<AICharacterSpawner> aiCharacterSpawners;
    [SerializeField] List<GameObject> spawnedAIs;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        
    }

    public void SpawnCharacter(AICharacterSpawner spawner)
    {
        aiCharacterSpawners.Add(spawner);
        spawner.AttemptToSpawnCharacter();
    }

    private void DespawnAllCharacters()
    {
        // TODO: Need to change this to work with spawner
        foreach (GameObject ai in spawnedAIs)
        {
            Destroy(ai);
        }
        spawnedAIs.Clear();
    }

    private void DisableAllCharacters()
    {
    }

    private void EnableAllCharacters()
    {
    }
}
