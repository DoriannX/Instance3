using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{

    [SerializeField] private List<SO_Room> roomsData;

    [SerializeField] private List<GameObject> roomsInLevel; //( [0] = entrée, [Last] = Armurerie) nb de room entre 2 et ?
    [SerializeField] private List<GameObject> roomsSpawned; //( [0] = entrée, [Last] = Armurerie) nb de room entre 2 et ?

    [SerializeField] private int maxRoomNumber = 10;
    [SerializeField] private int minRoomNumber = 3; // doesn't count the entrance and weaponry 
    [SerializeField] private int nbRoomInLevel;

    [SerializeField] private int currentLevel;

    [SerializeField] private Vector3 roomPositiontracker;

    [SerializeField] private float roomSpawnOffset = 100;
    [SerializeField] private int lastDirection;

    public void Awake()
    {
        LevelCreator();
    }

    private void LevelCreator()
    {
        if (roomsInLevel == null) {
            roomsInLevel = new List<GameObject>();
        }
        else
        {
            roomsInLevel.Clear(); // clear all data of the previous level for the TrimExcess()
            roomsInLevel.TrimExcess(); // reduce the capacity back to 0 to have different amounts of room per level

            roomsInLevel = new List<GameObject>();
        }

        nbRoomInLevel = Random.Range(2 + minRoomNumber, maxRoomNumber); // amount of room to be created ( since entrance and exit are always present, the list contains at least two elements

        RandomRooms(0);

        for (int i = 1; i < nbRoomInLevel - 1; i++)
        {
            RandomRooms(i);
        }

        RandomRooms(-1);
    }

    private GameObject SpawnRoom(GameObject roomToSpawn, Vector3 pos)
    {
        GameObject SpawnedRoom = Instantiate(roomToSpawn, pos, new Quaternion(0, 0, 0, 0));
        Rooms rooms = SpawnedRoom.GetComponent<Rooms>();
        if (roomsSpawned.Count > 0)
        {
            rooms.connectedRooms.Add(roomsSpawned[roomsSpawned.Count - 1]);
            rooms.CreateCorridor(3 - lastDirection);
        }
        roomsSpawned.Add(SpawnedRoom);
        roomPositiontracker = pos;
        return SpawnedRoom;
    }

    public GameObject RandomRooms(int roomIndex)
    {
        if (roomIndex == 0)
        {
            roomsInLevel.Add(roomsData.Find(x => x.roomName == "Entrance").roomPrefab);
        }
        else if (roomIndex == - 1)
        {
            roomsInLevel.Add(roomsData.Find(x => x.roomName == "Weaponry").roomPrefab);
        }
        else
        {
            int roomRng = Random.Range(2, roomsData.Count);
            roomsInLevel.Add(roomsData[roomRng].roomPrefab);
        }
        Debug.Log(roomsInLevel[roomsInLevel.Count - 1]);
        return SpawnRoom(roomsInLevel[roomsInLevel.Count - 1], UsePositionTracker());
    }

    public Vector3 UsePositionTracker()
    {
        int directionRNG = Random.Range(0,4);
        Debug.Log(directionRNG);
        Debug.Log(lastDirection);
        while (directionRNG%2 == lastDirection)
        {
            directionRNG = Random.Range(0, 4);
        }
        switch (Random.Range(0, 4))
        {
            case 0: // Up
                roomPositiontracker += new Vector3( 0, 0, roomSpawnOffset);
                lastDirection = 0;
                break;

            case 1: // Right
                roomPositiontracker += new Vector3( roomSpawnOffset, 0, 0);
                lastDirection = 1;
                break;

            case 2: // Down
                roomPositiontracker += new Vector3( 0, 0, -roomSpawnOffset);
                lastDirection = 2;
                break;

            case 3: // Left
                roomPositiontracker += new Vector3( -roomSpawnOffset, 0, 0);
                lastDirection = 3;
                break;
        }

        return roomPositiontracker;
    }
}
 
