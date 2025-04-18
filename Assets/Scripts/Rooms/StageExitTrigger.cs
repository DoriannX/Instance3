using System;
using UnityEngine;

public class StageExitTrigger : MonoBehaviour
{
    [SerializeField] private GameObject nextStageEntranceSpawnPoint;

    public event Action onPlayerClearStage;
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            player.transform.position = nextStageEntranceSpawnPoint.transform.position;
            onPlayerClearStage?.Invoke();
            //LevelManager.LoadNextLevel();
        }
    }
}