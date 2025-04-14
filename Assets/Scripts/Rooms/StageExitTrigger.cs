using System;
using UnityEngine;

public class StageExitTrigger : MonoBehaviour
{
    public event Action onPlayerClearStage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            onPlayerClearStage?.Invoke();
            LevelManager.LoadNextLevel();
        }
    }
}