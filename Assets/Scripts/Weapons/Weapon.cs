using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Weapon Stats")]
    public string weaponName;
    public int damage;
    public float cooldown;      
}
