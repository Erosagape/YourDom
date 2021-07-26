﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RPGSample
{
    public class Chest : WorldObject,ICloneable
    {
        /// <summary>
        /// The amount of gold in the chest.
        /// </summary>
        private int gold = 0;

        /// <summary>
        /// The amount of gold in the chest.
        /// </summary>
        [ContentSerializer(Optional = true)]
        public int Gold
        {
            get { return gold; }
            set { gold = value; }
        }


        /// <summary>
        /// The gear in the chest, along with quantities.
        /// </summary>
        private List<ContentEntry<Gear>> entries = new List<ContentEntry<Gear>>();

        /// <summary>
        /// The gear in the chest, along with quantities.
        /// </summary>
        public List<ContentEntry<Gear>> Entries
        {
            get { return entries; }
            set { entries = value; }
        }


        /// <summary>
        /// Array accessor for the chest's contents.
        /// </summary>
        public ContentEntry<Gear> this[int index]
        {
            get { return entries[index]; }
        }


        /// <summary>
        /// Returns true if the chest is empty.
        /// </summary>
        public bool IsEmpty
        {
            get { return ((gold <= 0) && (entries.Count <= 0)); }
        }
        /// <summary>
        /// The content name of the texture for this chest.
        /// </summary>
        private string textureName;

        /// <summary>
        /// The content name of the texture for this chest.
        /// </summary>
        public string TextureName
        {
            get { return textureName; }
            set { textureName = value; }
        }


        /// <summary>
        /// The texture for this chest.
        /// </summary>
        private Texture2D texture;

        /// <summary>
        /// The texture for this chest.
        /// </summary>
        [ContentSerializerIgnore]
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }
        /// <summary>
        /// Clone implementation for chest copies.
        /// </summary>
        /// <remarks>
        /// The game has to handle chests that have had some contents removed
        /// without modifying the original chest (and all chests that come after).
        /// </remarks>
        public object Clone()
        {
            // create the new chest
            Chest chest = new Chest();

            // copy the data
            chest.Gold = Gold;
            chest.Name = Name;
            chest.Texture = Texture;
            chest.TextureName = TextureName;

            // recreate the list and entries, as counts may have changed
            chest.entries = new List<ContentEntry<Gear>>();
            foreach (ContentEntry<Gear> originalEntry in Entries)
            {
                ContentEntry<Gear> newEntry = new ContentEntry<Gear>();
                newEntry.Count = originalEntry.Count;
                newEntry.ContentName = originalEntry.ContentName;
                newEntry.Content = originalEntry.Content;
                chest.Entries.Add(newEntry);
            }

            return chest;
        }

    }
}
