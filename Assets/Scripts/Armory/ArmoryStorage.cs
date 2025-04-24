using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Armory
{
    public class ArmoryStorage : MonoBehaviour
    {
        [SerializeField, Min(0)] private int nbWeapons = 3;
        [Tooltip("All possible WeaponData assets to draw from at level end.")]
        [SerializeField] private List<WeaponData> possibleWeaponDatas;

        private WeaponData[] weaponsInStock;

        private void Awake()
        {
            FillStock();
        }

        private void FillStock()
        {
            if (nbWeapons == 0 || possibleWeaponDatas == null || possibleWeaponDatas.Count == 0)
            {
                Debug.LogWarning("ArmoryStorage: No weapon data to fill."); 
                weaponsInStock = Array.Empty<WeaponData>();
                return;
            }

            var pool = new List<WeaponData>(possibleWeaponDatas);
            weaponsInStock = new WeaponData[nbWeapons];

            for (int i = 0; i < nbWeapons; i++)
            {
                int r = Random.Range(0, pool.Count);
                weaponsInStock[i] = pool[r];
                pool.RemoveAt(r);
            }
        }

        /// <summary>
        /// Swaps out the weaponData at the given index with the provided one,
        /// returning the old data so the UI can put it back in the slot.
        /// </summary>
        public WeaponData SwitchWeaponData(WeaponData toStore, int index)
        {
            if (index < 0 || index >= weaponsInStock.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            WeaponData taken = weaponsInStock[index];
            weaponsInStock[index] = toStore;
            return taken;
        }

        public WeaponData[] WeaponsInStock => weaponsInStock;
    }
}