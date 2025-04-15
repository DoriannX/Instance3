using NUnit.Framework;
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

    [SerializeField] private Vector3 roomPositiontracker = new(0,0,0); //starting position of the level

    [SerializeField] private float roomSpawnOffset = 100;
    [SerializeField] private List<int> lastDirection; // so that any direction works for the first room
    [SerializeField] private List<Vector3> positionTaken; // so that any direction works for the first room

    public void Awake()
    {
        Assert.NotNull(roomsData);
        Assert.NotNull(roomsInLevel);
        Assert.GreaterOrEqual(maxRoomNumber,minRoomNumber);
        LevelCreator();
    }

    private void LevelCreator()
    {
        if (roomsInLevel == null)
        {
            roomsInLevel = new List<GameObject>();
        }
        else
        {
            roomsInLevel.Clear(); // clear all data of the previous level for the TrimExcess()
            roomsInLevel.TrimExcess(); // reduce the capacity back to 0 to have different amounts of room per level

            roomsInLevel = new List<GameObject>();
        }

        lastDirection.Clear();
        lastDirection.TrimExcess();
        lastDirection = new List<int>();

        nbRoomInLevel = Random.Range(2 + minRoomNumber, maxRoomNumber); // amount of room to be created ( since entrance and exit are always present, the list contains at least two elements

        RandomRooms(0); // spawn entry room in level

        for (int i = 1; i < nbRoomInLevel - 1; i++)
        {
            RandomRooms(i); // spawn general rooms in level
        }

        RandomRooms(-1); // spawn weaponry room in level
    }

    private GameObject SpawnRoom(GameObject roomToSpawn, Vector3 pos)
    {
        GameObject SpawnedRoom = Instantiate(roomToSpawn, pos, new Quaternion(0, 0, 0, 0));
        Rooms rooms = SpawnedRoom.GetComponent<Rooms>();
        if (roomsSpawned.Count > 0)
        {
            rooms.connectedRooms.Add(roomsSpawned[^1]);
            rooms.CreateCorridor(lastDirection[^1], (lastDirection[^1] + 2) % 4);
        }
        roomsSpawned.Add(SpawnedRoom);
        return SpawnedRoom;
    }

    public GameObject RandomRooms(int roomIndex)
    {
        if (roomIndex == 0)
        {
            roomsInLevel.Add(roomsData.Find(x => x.roomName == "Entrance").roomPrefab);
        }
        else if (roomIndex == -1)
        {
            roomsInLevel.Add(roomsData.Find(x => x.roomName == "Weaponry").roomPrefab);
        }
        else
        {
            int roomRng = Random.Range(2, roomsData.Count);
            roomsInLevel.Add(roomsData[roomRng].roomPrefab);
        }
        Vector3 pos = UsePositionTracker();
        return SpawnRoom(roomsInLevel[^1], pos);
    }

    public Vector3 UsePositionTracker()
    {
        switch (DirectionSelector()) // if direction is left the room will spawn to the right and it's entry be on the left side of the room
        {
            case 0: // Left
                if (!IsPositionFree(0))
                {
                    UsePositionTracker();
                    break;
                }
                roomPositiontracker += new Vector3(roomSpawnOffset, 0, 0);
                lastDirection.Add(0);
                break;

            case 1: // Up
                if (!IsPositionFree(1))
                {
                    UsePositionTracker();
                    break;
                }
                roomPositiontracker += new Vector3(0, 0, -roomSpawnOffset);
                lastDirection.Add(1);
                break;

            case 2: // Right
                if (!IsPositionFree(2))
                {
                    UsePositionTracker();
                    break;
                }
                roomPositiontracker += new Vector3(-roomSpawnOffset, 0, 0);
                lastDirection.Add(2);
                break;

            case 3: // Down
                if (!IsPositionFree(3))
                {
                    UsePositionTracker();
                    break;
                }
                roomPositiontracker += new Vector3(0, 0, roomSpawnOffset);
                lastDirection.Add(3);
                break;
        }
        positionTaken.Add(roomPositiontracker);
        return roomPositiontracker;
    }

    public int DirectionSelector()
    {
        int directionRNG = Random.Range(0, 4);

        if (lastDirection.Count > 4)
        {
            while ((directionRNG + 2) % 4 == lastDirection[^1] || (directionRNG + 2) % 4 == lastDirection[^4])
            {
                directionRNG = Random.Range(0, 4);
            }
            return directionRNG;
        }
        if (lastDirection.Count > 1)
        {
            while ((directionRNG + 2) % 4 == lastDirection[^1])
            {
                directionRNG = Random.Range(0, 4);
            }
            return directionRNG;
        }

        return directionRNG;
    }

    public bool IsPositionFree(int i)
    {
        switch (i)
        {
            case 0: // Left
                foreach (GameObject room in roomsSpawned)
                {
                    if (roomPositiontracker + new Vector3(roomSpawnOffset, 0, 0) == room.transform.position)
                    {
                        return false;
                    }
                }
                break;

            case 1: // Up
                foreach (GameObject room in roomsSpawned)
                {
                    if (roomPositiontracker + new Vector3(0, 0, -roomSpawnOffset) == room.transform.position)
                    {
                        return false;
                    }
                }
                break;

            case 2: // Right
                foreach (GameObject room in roomsSpawned)
                {
                    if (roomPositiontracker + new Vector3(-roomSpawnOffset, 0, 0) == room.transform.position)
                    {
                        return false;
                    }
                }
                break;

            case 3: // Down
                foreach (GameObject room in roomsSpawned)
                {
                    if (roomPositiontracker + new Vector3(0, 0, roomSpawnOffset) == room.transform.position)
                    {
                        return false;
                    }
                }
                break;
        }
        return true;
    }
}

