using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Rooms : MonoBehaviour
{
    [SerializeField] private SO_Room Room;
    [SerializeField] private List<GameObject> SpawnPointEnemy;
    [SerializeField] private int SpawnTracker = 0;
    [SerializeField] private GameObject[] RoomTranstions;
    public void RoomInitialization()
    {
        if (Room.Nbenemy.Sum() != SpawnPointEnemy.Count)
        {
            Debug.Log("The number of spawnPoint isn't enough you need :" + Room.Nbenemy.Sum() + $" psawn point in {Room.name}");
        }

        for(int i = 0; i < Mathf.Min(Room.Nbenemy.Count,Room.EnemyType.Count); i++)
        {
            for( int j = SpawnTracker; j < Room.Nbenemy[i]; j++)
            {
                SpawnTracker = j;
                Instantiate(Room.EnemyType[i], SpawnPointEnemy[SpawnTracker].transform);
            }
        }
    }
}
