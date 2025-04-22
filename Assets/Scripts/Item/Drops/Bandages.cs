using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Item.Drops
{
    public class Bandages : ItemDrop
    {
        [FormerlySerializedAs("healAmnt")] [SerializeField] private uint healAmount = 10;

        public override void ApplyEffect()
        {
            if (targetPlayer is not Player player)
            {
                throw new InvalidCastException($"Wrong Entity: {targetPlayer}");
            }

            player.healthComponent.Heal((int)healAmount);
        }
        
        public void SetHealAmount(uint amount)
        {
            healAmount = amount;
        }
    }
}