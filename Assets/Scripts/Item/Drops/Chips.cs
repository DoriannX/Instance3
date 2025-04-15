using System;

namespace Item.Drops
{
    public class Chips : ItemDrop
    {
        private uint chipsAmount;

        public override void ApplyEffect()
        {
            if (targetPlayer is not Player player)
            {
                throw new InvalidCastException($"Wrong Entity: {targetPlayer}");
            }
            player.AddChips((int)chipsAmount);
        }
        
        public void SetChipsAmount(uint amount)
        {
            chipsAmount = amount;
        }
    }
}