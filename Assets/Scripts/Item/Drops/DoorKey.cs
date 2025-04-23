using System;
using UnityEngine;

namespace Item.Drops
{
    public class DoorKey : ItemDrop
    {
        public static bool isKeyDropped = false;

        protected override void Start()
        {
            base.Start();
            Debug.LogWarning("DoorKey Start");
            isKeyDropped = true;
        }

        public override void ApplyEffect()
        {
            targetPlayer.HasKey(true);
        }

        private void OnDestroy()
        {
            isKeyDropped = false;
        }
    }
}