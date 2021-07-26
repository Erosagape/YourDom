using System;
using System.Collections.Generic;

namespace RPGSample
{
    public class FixedCombat : WorldObject
    {
        /// <summary>
        /// The content name and quantities of the monsters in this encounter.
        /// </summary>
        private List<ContentEntry<Monster>> entries = new List<ContentEntry<Monster>>();

        /// <summary>
        /// The content name and quantities of the monsters in this encounter.
        /// </summary>
        public List<ContentEntry<Monster>> Entries
        {
            get { return entries; }
            set { entries = value; }
        }
    }
}
