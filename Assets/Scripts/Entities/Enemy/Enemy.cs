using System;

public class Enemy : Entity
{
    public static int aliveEnemyCount { get; private set; } = 0;

    private void Start()
    {
        healthComponent.onDeath += OnEnemyDie;
        aliveEnemyCount++;
    }

    private void OnEnemyDie()
    {
        Destroy(gameObject);
        aliveEnemyCount--;
    }
}