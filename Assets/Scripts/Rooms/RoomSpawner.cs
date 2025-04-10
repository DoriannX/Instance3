using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    [SerializeField] private SO_RoomGenerator RoomSpwn;

    public void Awake()
    {
        Instantiate(RoomSpwn);
    }
}
