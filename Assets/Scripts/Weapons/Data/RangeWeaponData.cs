using UnityEngine;

[CreateAssetMenu(fileName = "RangeWeaponData", menuName = "Weapons/RangeWeapon")]
public class RangeWeaponData : WeaponData
{  
    public int bulletSpread = 2;
    public int ammoConsumme;
}
