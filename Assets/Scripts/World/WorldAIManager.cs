using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WorldAIManager : MonoBehaviour
{
    public static WorldAIManager instance;

    [Header("Debug")]
    [SerializeField] bool despawnCharacters = false;
    [SerializeField] bool respawnCharacters = false;

    [Header("AIs")]
    [SerializeField] List<AICharacterSpawner> aiCharacterSpawners;
    [SerializeField] List<AICharacterManager> spawnedAIs;

    [Header("Bosses")]
    [SerializeField] List<AIBossCharacterManager> spawnedBosses;

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

    public void AddSpawnedCharacter(AICharacterManager character)
    {
        if (!spawnedAIs.Contains(character))
        {
            spawnedAIs.Add(character);

            if (character is AIBossCharacterManager)
            {
                spawnedBosses.Add((AIBossCharacterManager) character);
            }
        }
    }

    public AIBossCharacterManager GetBossByID(int bossID)
    {
        return spawnedBosses.FirstOrDefault(boss => boss.bossID == bossID);
    }

    private void DespawnAllCharacters()
    {
        // TODO: Need to change this to work with spawner
        foreach (AICharacterManager ai in spawnedAIs)
        {
            Destroy(ai.gameObject);
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
