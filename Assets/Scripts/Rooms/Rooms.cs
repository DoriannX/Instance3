using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class Rooms : MonoBehaviour
{
    [SerializeField] private SO_Room room;

    [Header("EnemySpawn")] [SerializeField]
    private GameObject spawnPointEnemy;

    [SerializeField] private int spawnTracker;

    [Header("RoomExitPoints")] [SerializeField]
    private List<GameObject> roomTranstions;

    public void Start()
    {
        RoomInitialization();
    }

    public void RoomInitialization()
    {
        if (room.nbEnemy.Sum() > spawnPointEnemy.transform.childCount)
        {
            Debug.LogError("The number of spawnPoint isn't enough. You need: " + room.nbEnemy.Sum() +
                           $" spawn points in {room.name}");
        }

        for (int i = 0; i < Mathf.Min(room.nbEnemy.Count, room.enemyType.Count); i++)
        {
            int currentSpawnTracker = spawnTracker;
            for (int j = 0; j < room.nbEnemy[i]; j++)
            {
                GameObject SpawnedEnemy = Instantiate(room.enemyType[i].enemy,
                    spawnPointEnemy.transform.GetChild(spawnTracker).transform);
                spawnTracker++;
            }
        }
    }
}