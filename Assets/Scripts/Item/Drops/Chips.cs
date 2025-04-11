using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Item.Drops
{
    public class Chips : ItemDrop
    {
        [FormerlySerializedAs("chipsAmnt")] [SerializeField]
        private uint chipsAmount;

        public override void ApplyEffect()
        {
            if (targetPlayer is not Player player)
            {
                throw new InvalidCastException($"Wrong Entity: {targetPlayer}");
            }
            player.AddChips((int)chipsAmount);
        }
    }
}