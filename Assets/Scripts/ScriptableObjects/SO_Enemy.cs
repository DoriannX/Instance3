using UnityEngine;

[CreateAssetMenu(fileName = "SO_Enemy", menuName = "ScriptableObjects/SO_enemy", order = 3)]
public class SO_Enemy : ScriptableObject
{
    public int Hp;
    public string Name;
    public float PatrolSpd;
    public float ChaseSpd;
    public GameObject Enemy;
}
