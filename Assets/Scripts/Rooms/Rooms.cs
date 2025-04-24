using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class Rooms : MonoBehaviour
{
    [SerializeField] private SO_Room room;

    //[Header("EnemySpawn")] [SerializeField]
    //private GameObject spawnPointEnemy;

    [SerializeField] private int spawnTracker;

    [Header("RoomExitPoints")] [SerializeField]
    private List<GameObject> roomTranstions;
    
    [field : SerializeField] public float zoomCameraData { get; private set; }
    [field : SerializeField] public Vector3 positionCameraData { get; private set; }
    [field : SerializeField] public Vector3 rotationCameraData { get; private set; }

    public void Start()
    {
        LoadData();
        //RoomInitialization();
        Assert.IsNotNull(room, "Room is not assigned.");
    }

    //public void RoomInitialization()
    //{
    //    if (room.nbEnemy.Sum() > spawnPointEnemy.transform.childCount)
    //    {
    //        Debug.LogError("The number of spawnPoint isn't enough. You need: " + room.nbEnemy.Sum() +
    //                       $" spawn points in {room.name}");
    //    }

    //    for (int i = 0; i < Mathf.Min(room.nbEnemy.Count, room.enemyType.Count); i++)
    //    {
    //        int currentSpawnTracker = spawnTracker;
    //        for (int j = 0; j < room.nbEnemy[i]; j++)
    //        {
    //            GameObject SpawnedEnemy = Instantiate(room.enemyType[i].enemy,
    //                spawnPointEnemy.transform.GetChild(spawnTracker).transform);
    //            spawnTracker++;
    //        }
    //    }
    //}

    private void LoadData()
    {
        zoomCameraData = Mathf.Clamp(room.zoomCameraData, 0.1f, 100f);
    }
}