using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;

[CreateAssetMenu(menuName = "Enemy Spawning Config")]
public class SpawnConfig : ScriptableObject
{
    // Config Parameters
    [SerializeField] List<GameObject> enemyPrefabs;
    [SerializeField] List<GameObject> enemyPathings;
    [SerializeField] int spawnNumber;
    [SerializeField] int spawningCount;
    [SerializeField] GameObject roomLock;

    // Dyanamic Variables
    
    public GameObject GetEnemyPrefab(System.Random rnd)
    {
        int index = rnd.Next() % enemyPrefabs.Count;
        return enemyPrefabs[index];
    }

    public List<Transform> GetRandomPath(System.Random rnd)
    {
        List<Transform> waypoints = new List<Transform>();
        GameObject enemyPath;
        int index = rnd.Next() % enemyPathings.Count;
        enemyPath = enemyPathings[index];

        foreach(Transform child in enemyPath.transform)
        {
            waypoints.Add(child);
        }
        return Shuffle(waypoints, rnd);
    }

    public int GetSpawnNumber()
    {
        return spawnNumber;
    }

    public int GetSpawningCount()
    {
        return spawningCount;
    }

    public static List<T> Shuffle<T>(List<T> list, System.Random rnd)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int k = rnd.Next(0, i);
            T value = list[k];
            list[k] = list[i];
            list[i] = value;
        }
        return list;
    }

    public GameObject ReturnRoomLockGO()
    {
        GameObject instanceRoomLockGO = Instantiate(roomLock, roomLock.transform.position, Quaternion.identity);
        return instanceRoomLockGO;
    }
}
