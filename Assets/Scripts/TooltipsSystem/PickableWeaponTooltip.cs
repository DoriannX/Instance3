using Item;
using UnityEngine;
using UnityEngine.Assertions;

namespace TooltipsSystem
{
    public class PickableWeaponTooltip : Tooltip
    {
        [SerializeField] private PickableWeapon pickableWeapon;

        protected override void Awake()
        {
            base.Awake();
            Assert.IsNotNull(pickableWeapon);
        }

        protected override void Start()
        {
            base.Start();
            OpenTooltip();
            pickableWeapon.onItemStartPickup += PickableWeaponOnonItemStartPickup;
        }

        private void PickableWeaponOnonItemStartPickup()
        {
            CloseTooltip();
        }
    }
}