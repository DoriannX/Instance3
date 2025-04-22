using UnityEngine;

public class WeaponData : ScriptableObject
{
    public Weapon weaponPrefab;
    public string weaponName;
    public string description;
    public Mesh mesh;
    public int damage;
    public float cooldown;
    public float attackRange;
    public string attackSFX;
    public float knockbackForce;
    public Sprite icon;
}
