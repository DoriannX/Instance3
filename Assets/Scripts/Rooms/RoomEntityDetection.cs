using System;
using System.Collections.Generic;
using Entities.Enemy.BehaviorTree;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RoomEntityDetection : MonoBehaviour
{
    private BoxCollider detectionBox;
    
    private List<EnemyBT> enemiesInRoom = new List<EnemyBT>();
    
    private void Awake()
    {
        detectionBox = GetComponent<BoxCollider>() ?? throw new ArgumentNullException(nameof(detectionBox));
        detectionBox.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyBT enemy))
        {
            if (!enemiesInRoom.Contains(enemy))
                enemiesInRoom.Add(enemy);
        }
        else if (!other.isTrigger && other.TryGetComponent(out Player player))
        {
            AlertEnemies(player.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.isTrigger && other.TryGetComponent(out Player _))
        {
            AlertEnemies(null);
        }
    }

    private void AlertEnemies(Transform player)
    {
        foreach (var enemy in enemiesInRoom)
        {
            enemy.SetTargetInRoom(player);
        }
    }
}