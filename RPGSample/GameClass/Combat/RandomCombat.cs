using System;
using System.Collections.Generic;

namespace RPGSample
{
    public class RandomCombat
    {
        /// <summary>
        /// The chance of a random combat starting with each step, from 1 to 100.
        /// </summary>
        private int combatProbability;

        /// <summary>
        /// The chance of a random combat starting with each step, from 1 to 100.
        /// </summary>
        public int CombatProbability
        {
            get { return combatProbability; }
            set { combatProbability = value; }
        }


        /// <summary>
        /// The chance of a successful escape from a random combat, from 1 to 100.
        /// </summary>
        private int fleeProbability;

        /// <summary>
        /// The chance of a successful escape from a random combat, from 1 to 100.
        /// </summary>
        public int FleeProbability
        {
            get { return fleeProbability; }
            set { fleeProbability = value; }
        }


        /// <summary>
        /// The range of possible quantities of monsters in the random encounter.
        /// </summary>
        private Int32Range monsterCountRange;

        /// <summary>
        /// The range of possible quantities of monsters in the random encounter.
        /// </summary>
        public Int32Range MonsterCountRange
        {
            get { return monsterCountRange; }
            set { monsterCountRange = value; }
        }


        /// <summary>
        /// The monsters that might be in the random encounter, 
        /// along with quantity and weight.
        /// </summary>
        private List<WeightedContentEntry<Monster>> entries =
            new List<WeightedContentEntry<Monster>>();

        /// <summary>
        /// The monsters that might be in the random encounter, 
        /// along with quantity and weight.
        /// </summary>
        public List<WeightedContentEntry<Monster>> Entries
        {
            get { return entries; }
            set { entries = value; }
        }
    }
}
