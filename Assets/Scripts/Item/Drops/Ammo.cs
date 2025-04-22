using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Item.Drops
{
    public class Ammo : ItemDrop
    {
        [FormerlySerializedAs("ammoAmnt")] [SerializeField] private uint ammoAmount;

        public override void ApplyEffect()
        {
            if (targetPlayer is not Player player)
            {
                throw new InvalidCastException($"Wrong Entity: {targetPlayer}");
            }
            player.GatherAmmo((int)ammoAmount);
        }
    }
}