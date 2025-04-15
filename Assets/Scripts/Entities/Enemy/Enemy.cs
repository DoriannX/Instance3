public class Enemy : Entity
{
    public SO_Enemy enemyType;
    
    private void Start()
    {
        healthComponent.onDeath += OnEnemyDie;
    }

    private void OnEnemyDie()
    {
        Destroy(gameObject);
    }
}
