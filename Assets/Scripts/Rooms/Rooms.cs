using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Rooms : MonoBehaviour
{
    public SO_Room room;
    [Header("EnemySpawn")]
    [SerializeField] private GameObject spawnPointEnemy;
    [SerializeField] private int spawnTracker;
    [Header("RoomExitPoints")]
    public List<GameObject> connectedRooms;
    [SerializeField] private List<GameObject> corridors;

    public void Start()
    {
        RoomInitialization();
    }

    public void RoomInitialization()
    {
        Assert.GreaterOrEqual(room.nbEnemy.Sum(),spawnPointEnemy.transform.childCount, "The number of spawnPoint isn't enough you need :" + room.nbEnemy.Sum() + $" spawn point in {room.name}");

        for(int i = 0; i < Mathf.Min(room.nbEnemy.Count,room.enemyType.Count); i++)
        {
            int currentSpawnTracker = spawnTracker;
            for( int j = 0; j < room.nbEnemy[i]; j++)
            {
                GameObject SpawnedEnemy = Instantiate(room.enemyType[i].enemy, spawnPointEnemy.transform.GetChild(spawnTracker).transform);
                spawnTracker++;
            }
        }
    }

    public GameObject CreateCorridor(int exitIndex)
    {
        room.corridorPrefab.GetComponent<Corridor>().corridorStart = corridors[exitIndex];
        room.corridorPrefab.GetComponent<Corridor>().corridorEnd = connectedRooms[0].GetComponent<Rooms>().corridors[exitIndex];
        GameObject corridor = Instantiate(room.corridorPrefab);

        return corridor;
    }
}
