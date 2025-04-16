using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    [SerializeField] private List<SO_Room> roomsData;

    [SerializeField]
    private List<GameObject> roomsInLevel; //( [0] = entr�e, [Last] = Armurerie) nb de room entre 2 et ?

    [SerializeField] private List<GameObject> roomsSpawned;
    [SerializeField] private List<GameObject> corridorsSpawned;

    [SerializeField] private int maxRoomNumber = 10;
    [SerializeField] private int minRoomNumber = 3; // doesn't count the entrance and weaponry 
    [SerializeField] private int nbRoomInLevel;

    [SerializeField] private int currentLevel;

    [SerializeField] private Vector3 roomPositiontracker = new(0, 0, 0); //starting position of the level

    [SerializeField] private float roomSpawnOffset = 100;
    [SerializeField] private List<int> lastDirection; // so that any direction works for the first room
    [SerializeField] private List<Vector3> positionTaken; // so that any direction works for the first room

    public void Awake()
    {
        Assert.NotNull(roomsData);
        Assert.NotNull(roomsInLevel);
        Assert.GreaterOrEqual(maxRoomNumber, minRoomNumber);
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

        nbRoomInLevel =
            Random.Range(2 + minRoomNumber,
                maxRoomNumber); // amount of room to be created ( since entrance and exit are always present, the list contains at least two elements

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
            corridorsSpawned.Add(rooms.CreateCorridor(lastDirection[^1], (lastDirection[^1] + 2) % 4));
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
        int direction = DirectionSelector();
        if (!IsPositionFree(direction))
        {
            return UsePositionTracker(); // Recursive call to try another direction
        }

        Vector3 offset = direction switch
        {
            0 => new Vector3(roomSpawnOffset, 0, 0), // Left
            1 => new Vector3(0, 0, -roomSpawnOffset), // Up
            2 => new Vector3(-roomSpawnOffset, 0, 0), // Right
            3 => new Vector3(0, 0, roomSpawnOffset), // Down
            _ => Vector3.zero
        };

        roomPositiontracker += offset;
        lastDirection.Add(direction);
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
        Vector3 offset = i switch
        {
            0 => new Vector3(roomSpawnOffset, 0, 0), // Left
            1 => new Vector3(0, 0, -roomSpawnOffset), // Up
            2 => new Vector3(-roomSpawnOffset, 0, 0), // Right
            3 => new Vector3(0, 0, roomSpawnOffset), // Down
            _ => Vector3.zero
        };

        foreach (GameObject room in roomsSpawned)
        {
            if (roomPositiontracker + offset == room.transform.position)
            {
                return false;
            }
        }

        return true;
    }

    public void ClearLevel() // to be called between level changes
    {
        foreach (GameObject corridor in corridorsSpawned)
        {
            Destroy(corridor);
        }

        foreach (GameObject room in roomsSpawned)
        {
            Destroy(room);
        }
    }
}