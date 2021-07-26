using System;
using System.Collections.Generic;
namespace RPGSample
{
    public class ModifiedChestEntry
    {
        /// <summary>
        /// The map and position of the modified chest.
        /// </summary>
        public WorldEntry<Chest> WorldEntry = new WorldEntry<Chest>();

        /// <summary>
        /// The modified chest contents, replacing the previous contents.
        /// </summary>
        public List<ContentEntry<Gear>> ChestEntries = new List<ContentEntry<Gear>>();

        /// <summary>
        /// The modified gold amount in the chest, replacing the previous amount.
        /// </summary>
        public int Gold = 0;
    }
}
