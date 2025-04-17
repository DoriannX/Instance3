using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace Item
{
    public class PickableWeapon : ItemDrop
    {
        [FormerlySerializedAs("weaponToTake")] [SerializeField] private Weapon weaponToTakePrefab;

        protected override void Awake()
        {
            base.Awake();
            Assert.IsNotNull(weaponToTakePrefab);
        }

        public override void ApplyEffect()
        {
            Weapon weaponToTakeInstance = Instantiate(weaponToTakePrefab, targetPlayer.transform);
            targetPlayer.TakeWeapon(weaponToTakeInstance);
        }
    }
}