using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace RPGSample
{
    public class CharacterLevelDescription
    {
        /// <summary>
        /// The amount of additional experience necessary to achieve this level.
        /// </summary>
        private int experiencePoints;

        /// <summary>
        /// The amount of additional experience necessary to achieve this level.
        /// </summary>
        public int ExperiencePoints
        {
            get { return experiencePoints; }
            set { experiencePoints = value; }
        }

        /// <summary>
        /// The content names of the spells given to the character 
        /// when it reaches this level.
        /// </summary>
        private List<string> spellContentNames = new List<string>();

        /// <summary>
        /// The content names of the spells given to the character 
        /// when it reaches this level.
        /// </summary>
        public List<string> SpellContentNames
        {
            get { return spellContentNames; }
            set { spellContentNames = value; }
        }


        /// <summary>
        /// Spells given to the character when it reaches this level.
        /// </summary>
        private List<Spell> spells = new List<Spell>();

        /// <summary>
        /// Spells given to the character when it reaches this level.
        /// </summary>
        [ContentSerializerIgnore]
        public List<Spell> Spells
        {
            get { return spells; }
            set { spells = value; }
        }

    }
}
