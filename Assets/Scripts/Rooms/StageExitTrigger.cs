using System;
using UnityEngine;

public class StageExitTrigger : MonoBehaviour
{
    [SerializeField] private GameObject nextStageEntranceSpawnPoint;

    public event Action onPlayerClearStage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            onPlayerClearStage?.Invoke();
            LevelManager.LoadNextLevel();
        }
    }

    public void TeleportPlayerToNextLevel(GameObject player)
    {
        player.transform.position = nextStageEntranceSpawnPoint.transform.position;
        onPlayerClearStage?.Invoke();
        LevelManager.LoadNextLevel();
    }
}