using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_RoomGenerator", menuName = "ScriptableObjects/SO_RoomGenerator", order = 0)]
public class SO_RoomGenerator : ScriptableObject
{
    public List<SO_Room> RoomList;

    public GameObject SpawnRoom(int RoomIndex, Transform pos)
    {
        GameObject SpawnedRoom = Instantiate(RoomList[RoomIndex].RoomPrefab, pos);



        return SpawnedRoom;
    }
}
