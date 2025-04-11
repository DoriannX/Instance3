using UnityEngine;

public class Enemy : Entity
{
    public SO_Enemy EnemyType;

    public void Awake()
    {
        Debug.Log(gameObject.name + " has been instanciated with the enemy type : " + EnemyType.EnemyName);
    }
}
