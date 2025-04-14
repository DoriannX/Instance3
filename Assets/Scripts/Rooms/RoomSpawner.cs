using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    [SerializeField] private SO_RoomGenerator roomSpawn;

    public void Awake()
    {
        Instantiate(roomSpawn);
    }
}
