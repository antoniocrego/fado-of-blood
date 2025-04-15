// using UnityEngine;

// public class WorldAIManager : MonoBehaviour
// {
//     public static WorldAIManager instance;

//     [Header("AIs")]
//     [SerializeField] GameObject[] aiPrefabs;
//     [SerializeField] GameObject[] spawnedAIs;

//     private void Awake()
//     {
//         if (instance == null)
//         {
//             instance = this;
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }

//     private void Start()
//     {
//         spawnedAIs = new GameObject[aiPrefabs.Length];
//         for (int i = 0; i < aiPrefabs.Length; i++)
//         {
//             spawnedAIs[i] = Instantiate(aiPrefabs[i], transform.position, Quaternion.identity);
//             spawnedAIs[i].SetActive(false);
//         }
//     }
// }
