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
    [SerializeField] GameObject[] worldAIs;
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
        StartCoroutine(WaitForSceneToLoadThenSpawnCharacters());
    }

    private IEnumerator WaitForSceneToLoadThenSpawnCharacters()
    {
        while (!SceneManager.GetActiveScene().isLoaded)
        {
            yield return null;
        }
        SpawnAllCharacters();
    }

    private void SpawnAllCharacters()
    {
        foreach (GameObject ai in worldAIs)
        {
            GameObject spawnedAI = Instantiate(ai, ai.transform.position, Quaternion.identity);
            spawnedAI.transform.SetParent(transform);
            spawnedAIs.Add(spawnedAI);
        }
    }

    private void DespawnAllCharacters()
    {
        foreach (GameObject ai in spawnedAIs)
        {
            Destroy(ai);
        }
        spawnedAIs.Clear();
    }

    private void Update()
    {
        if (despawnCharacters)
        {
            DespawnAllCharacters();
            despawnCharacters = false;
        }

        if (respawnCharacters)
        {
            SpawnAllCharacters();
            respawnCharacters = false;
        }
    }

    private void DisableAllCharacters()
    {
    }

    private void EnableAllCharacters()
    {
    }
}
