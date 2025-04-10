using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Armory
{
    public class ArmoryStorage : MonoBehaviour
    {
        [SerializeField, Min(0)] private int nbWeapons = 3;
        [SerializeField] private List<Weapon> possibleWeapons;
        
        private Weapon[] weaponsInStock;
        
        private void Awake()
        {
            weaponsInStock = new Weapon[nbWeapons];
            FillStock();
        }

        private void FillStock()
        {
            if (nbWeapons == 0)
            {
                Debug.LogWarning("No weapons to fill in the stock.");
                return;
            }
            
            List<Weapon> availableWeapons = new List<Weapon>(possibleWeapons);
            
            for (int i = 0; i < nbWeapons; i++)
            {
                int randomIndex = Random.Range(0, availableWeapons.Count - 1);
                weaponsInStock[i] = availableWeapons[randomIndex];
                availableWeapons.RemoveAt(randomIndex);
            }
        }

        public Weapon SwitchWeapon(Weapon weaponToStore, int indexWeaponToTake)
        {
            if (weaponsInStock.Length == 0)
            {
                Debug.LogWarning("No weapons available in the stock.");
                return null;
            }

            if (indexWeaponToTake < 0 || indexWeaponToTake >= weaponsInStock.Length)
                throw new ArgumentOutOfRangeException(nameof(indexWeaponToTake), "Index is out of range");

            Weapon newWeapon = weaponsInStock[indexWeaponToTake];
            weaponsInStock[indexWeaponToTake] = weaponToStore;
            return newWeapon;
        }
        
        public void SetNbAvailableWeapons(int nbWeapons)
        {
            if (nbWeapons < 0)
                throw new ArgumentOutOfRangeException(nameof(nbWeapons), "Number of weapons cannot be negative");
            
            this.nbWeapons = nbWeapons;
            FillStock();
        }
        
        public Weapon[] WeaponsInStock => weaponsInStock;
    }
}