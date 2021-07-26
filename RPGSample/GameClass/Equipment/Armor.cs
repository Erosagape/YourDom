using System;
namespace RPGSample
{
    public class Armor : Equipment
    {
        /// <summary>
        /// Slots that a piece of armor may fill on a character.
        /// </summary>
        /// <remarks>Only one piece may fill a slot at the same time.</remarks>
        public enum ArmorSlot
        {
            Helmet,
            Shield,
            Torso,
            Boots,
        };


        /// <summary>
        /// The slot that this armor fills.
        /// </summary>
        private ArmorSlot slot;

        /// <summary>
        /// The slot that this armor fills.
        /// </summary>
        public ArmorSlot Slot
        {
            get { return slot; }
            set { slot = value; }
        }
        /// <summary>
        /// Builds and returns a string describing the power of this armor.
        /// </summary>
        public override string GetPowerText()
        {
            return "Weapon Defense: " + OwnerHealthDefenseRange.ToString() +
                "\nMagic Defense: " + OwnerMagicDefenseRange.ToString();
        }
        /// <summary>
        /// The range of health defense provided by this armor to its owner.
        /// </summary>
        private Int32Range ownerHealthDefenseRange;

        /// <summary>
        /// The range of health defense provided by this armor to its owner.
        /// </summary>
        public Int32Range OwnerHealthDefenseRange
        {
            get { return ownerHealthDefenseRange; }
            set { ownerHealthDefenseRange = value; }
        }


        /// <summary>
        /// The range of magic defense provided by this armor to its owner.
        /// </summary>
        private Int32Range ownerMagicDefenseRange;

        /// <summary>
        /// The range of magic defense provided by this armor to its owner.
        /// </summary>
        public Int32Range OwnerMagicDefenseRange
        {
            get { return ownerMagicDefenseRange; }
            set { ownerMagicDefenseRange = value; }
        }

    }
}
