using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Rooms : MonoBehaviour
{
    [SerializeField] private SO_Room Room;
    [Header("EnemySpawn")]
    [SerializeField] private GameObject SpawnPointEnemy;
    [SerializeField] private int SpawnTracker;
    [Header("RoomExitPoints")]
    [SerializeField] private List<GameObject> RoomTranstions;

    public void Start()
    {
        RoomInitialization();
    }

    public void RoomInitialization()
    {
        if (Room.Nbenemy.Sum() > SpawnPointEnemy.transform.childCount)
        {
            Debug.Log("The number of spawnPoint isn't enough you need :" + Room.Nbenemy.Sum() + $" spawn point in {Room.name}");
            return;
        }

        for(int i = 0; i < Mathf.Min(Room.Nbenemy.Count,Room.EnemyType.Count); i++)
        {
            int currentSpawnTracker = SpawnTracker;
            //Debug.Log($"loop number : {currentSpawnTracker}");
            for( int j = 0; j < Room.Nbenemy[i]; j++)
            {
                //Debug.Log($"{j}th {Room.EnemyType[i].EnemyName} / at spawn number : {SpawnTracker}");
                GameObject SpawnedEnemy = Instantiate(Room.EnemyType[i].Enemy, SpawnPointEnemy.transform.GetChild(SpawnTracker).transform);
                SpawnTracker++;
            }
        }
    }
}
