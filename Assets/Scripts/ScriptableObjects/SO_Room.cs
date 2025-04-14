using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Room", menuName = "ScriptableObjects/SO_Room", order = 1)]
public class SO_Room : ScriptableObject
{
    public int roomID;
    public string roomName;
    public List<int> nbEnemy;
    public List<SO_Enemy> enemyType;
    public List<GameObject> exits;
    public GameObject roomPrefab;
    public GameObject corridorPrefab;
    public bool isSmallRoom;
}
