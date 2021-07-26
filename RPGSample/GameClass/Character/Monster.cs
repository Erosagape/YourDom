using System;
using System.Collections.Generic;

namespace RPGSample
{
    public class Monster : FightingCharacter
    {
        /// <summary>
        /// The chance that this monster will defend instead of attack.
        /// </summary>
        private int defendPercentage;

        /// <summary>
        /// The chance that this monster will defend instead of attack.
        /// </summary>
        public int DefendPercentage
        {
            get { return defendPercentage; }
            set { defendPercentage = (value > 100 ? 100 : (value < 0 ? 0 : value)); }
        }
        /// <summary>
        /// The possible gear drops from this monster.
        /// </summary>
        private List<GearDrop> gearDrops = new List<GearDrop>();

        /// <summary>
        /// The possible gear drops from this monster.
        /// </summary>
        public List<GearDrop> GearDrops
        {
            get { return gearDrops; }
            set { gearDrops = value; }
        }


        public int CalculateGoldReward(Random random)
        {
            return CharacterClass.BaseGoldValue * CharacterLevel;
        }


        public int CalculateExperienceReward(Random random)
        {
            return CharacterClass.BaseExperienceValue * CharacterLevel;
        }


        public List<string> CalculateGearDrop(Random random)
        {
            List<string> gearRewards = new List<string>();

            Random useRandom = random;
            if (useRandom == null)
            {
                useRandom = new Random();
            }

            foreach (GearDrop gearDrop in GearDrops)
            {
                if (random.Next(100) < gearDrop.DropPercentage)
                {
                    gearRewards.Add(gearDrop.GearName);
                }
            }

            return gearRewards;
        }

    }
}
