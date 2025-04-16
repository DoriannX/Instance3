using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu(fileName = "SO_RoomGenerator", menuName = "ScriptableObjects/SO_RoomGenerator", order = 0)]
public class SO_RoomGenerator : ScriptableObject
{
    public List<SO_Room> roomList;

    public GameObject SpawnRoom(int RoomIndex, Transform pos)
    {
        Assert.AreEqual(RoomIndex + 1, roomList.Count);
        GameObject SpawnedRoom = Instantiate(roomList[RoomIndex].roomPrefab, pos);

        return SpawnedRoom;
    }
}