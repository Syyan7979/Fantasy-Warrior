using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Config Parameters
    [SerializeField] SpawnConfig room;

    // Dynamic Variables
    int spawnNumber;
    int spawnCounter;
    RoomLock roomLock;
    System.Random rnd;

    // Start is called before the first frame update
    void Start()
    {
        spawnNumber = room.GetSpawnNumber();
        spawnCounter = room.GetSpawningCount();
        roomLock = FindObjectOfType<RoomLock>();
        rnd = new System.Random();
        Spawner(rnd);
    }

    // Update is called once per frame
    void Update()
    {
        SpawnNewWave();
    }

    void Spawner(System.Random rnd)
    {
        for (int i = 0; i < spawnNumber; i++)
        {
            var path = room.GetRandomPath(rnd);
            GameObject enemy = Instantiate(room.GetEnemyPrefab(rnd), path[0].transform.position, Quaternion.identity);
            enemy.GetComponent<EnemyPathing>().SetWaypoints(path);
        }
    }

    void SpawnNewWave()
    {
        if (spawnNumber == 0 && spawnCounter != 0)
        {
            spawnCounter--;
            roomLock.NewWaveSpawn();
            spawnNumber = room.GetSpawnNumber();
            if (spawnCounter >= 1)
            {
                StartCoroutine(SpawnNew());
            }
        }
    }

    IEnumerator SpawnNew()
    {
        yield return new WaitForSeconds(1.5f);
        Spawner(rnd);
    }

    public int GetSpawnWaveCount()
    {
        return room.GetSpawningCount();
    }

    public void SpawnKilled()
    {
        spawnNumber--;
    }
}
