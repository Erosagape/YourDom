using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace RPGSample
{
    public class StoreCategory
    {
        /// <summary>
        /// The display name of this store category.
        /// </summary>
        private string name;

        /// <summary>
        /// The display name of this store category.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }


        /// <summary>
        /// The content names for the gear available in this category.
        /// </summary>
        private List<string> availableContentNames = new List<string>();

        /// <summary>
        /// The content names for the gear available in this category.
        /// </summary>
        public List<string> AvailableContentNames
        {
            get { return availableContentNames; }
            set { availableContentNames = value; }
        }


        /// <summary>
        /// The gear available in this category.
        /// </summary>
        private List<Gear> availableGear = new List<Gear>();

        /// <summary>
        /// The gear available in this category.
        /// </summary>
        [ContentSerializerIgnore]
        public List<Gear> AvailableGear
        {
            get { return availableGear; }
            set { availableGear = value; }
        }

    }
}
