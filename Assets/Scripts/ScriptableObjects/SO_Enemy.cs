using UnityEngine;

[CreateAssetMenu(fileName = "SO_Enemy", menuName = "ScriptableObjects/SO_enemy", order = 3)]
public class SO_Enemy : ScriptableObject
{
    [Range(1,1000)]public int hp;
    public string enemyName;
    public float patrolSpd;
    public float chaseSpd;
    public GameObject enemy;
}
