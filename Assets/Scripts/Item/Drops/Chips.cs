using System;

namespace Item.Drops
{
    public class Chips : ItemDrop
    {
        public override void ApplyEffect()
        {
            if (targetPlayer is not Player player)
            {
                throw new InvalidCastException($"Wrong Entity: {targetPlayer}");
            }
            player.AddChip();
        }
    }
}