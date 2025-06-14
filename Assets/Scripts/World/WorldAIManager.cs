using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WorldAIManager : MonoBehaviour
{
    public static WorldAIManager instance;
    [Header("Loading")]
    public bool isPerformingLoadingOperation = false;
    private Coroutine spawnAllCharactersCoroutine;
    private Coroutine despawnAllCharactersCoroutine;
    private Coroutine resetAllCharactersCoroutine;
    

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

    public void ResetAllCharacters()
    {
        isPerformingLoadingOperation = true;

        if (resetAllCharactersCoroutine != null)
        {
            StopCoroutine(resetAllCharactersCoroutine);
        }

        resetAllCharactersCoroutine = StartCoroutine(ResetAllCharactersCoroutine());
    }

    private IEnumerator ResetAllCharactersCoroutine()
    {
        foreach (AICharacterSpawner spawner in  aiCharacterSpawners)
        {
            yield return new WaitForFixedUpdate();

            spawner.ResetCharacter();

            yield return null;
        }

        isPerformingLoadingOperation = false;

        yield return null;
    }

    private void SpawnAllCharacters()
    {
        isPerformingLoadingOperation = true;

        if (spawnAllCharactersCoroutine != null)
        {
            StopCoroutine(spawnAllCharactersCoroutine);
        }

        spawnAllCharactersCoroutine = StartCoroutine(SpawnAllCharactersCoroutine());
    }

    private IEnumerator SpawnAllCharactersCoroutine()
    {
        foreach (AICharacterSpawner spawner in aiCharacterSpawners)
        {
            yield return new WaitForFixedUpdate();

            spawner.AttemptToSpawnCharacter();

            yield return null;
        }

        isPerformingLoadingOperation = false;

        yield return null;
    }

    private void DespawnAllCharacters()
    {
        isPerformingLoadingOperation = true;

        if (despawnAllCharactersCoroutine != null)
        {
            StopCoroutine(despawnAllCharactersCoroutine);
        }

        despawnAllCharactersCoroutine = StartCoroutine(DespawnAllCharactersCoroutine());
    }

    private IEnumerator DespawnAllCharactersCoroutine()
    {
        foreach (AICharacterManager ai in spawnedAIs)
        {
            yield return new WaitForFixedUpdate();

            Destroy(ai.gameObject);

            yield return null;
        }

        spawnedAIs.Clear();
        spawnedBosses.Clear();

        isPerformingLoadingOperation = false;

        yield return null;
    }

    private void DisableAllCharacters()
    {
    }

    private void EnableAllCharacters()
    {
    }
}
