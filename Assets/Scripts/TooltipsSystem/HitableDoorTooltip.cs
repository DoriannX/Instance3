using Item;
using UnityEngine;

namespace TooltipsSystem
{
    public class HitableDoorTooltip : Tooltip
    {
        [SerializeField] private PickableWeaponTooltip pickableWeaponTooltip;
        [SerializeField] private HitableDoor door;

        protected override void Start()
        {
            base.Start();
            pickableWeaponTooltip.onTooltipClose += PickableWeaponTooltipOnonTooltipClose;
            door.onDoorOpened += CloseTooltip;
        }

        private void PickableWeaponTooltipOnonTooltipClose()
        {
            OpenTooltip();
        }
    }
}