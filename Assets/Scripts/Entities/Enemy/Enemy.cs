using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] private SO_Enemy EnemyType;

    public void Awake()
    {
        Debug.Log(gameObject.name + " has been instanciated with the enemy type : " + EnemyType.Name);
    }
}
