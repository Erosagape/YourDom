﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RPGSample
{
    public class Store : WorldObject
    {
        /// <summary>
        /// A purchasing multiplier applied to the price of all gear.
        /// </summary>
        private float buyMultiplier;

        /// <summary>
        /// A purchasing multiplier applied to the price of all gear.
        /// </summary>
        public float BuyMultiplier
        {
            get { return buyMultiplier; }
            set { buyMultiplier = value; }
        }


        /// <summary>
        /// A sell-back multiplier applied to the price of all gear.
        /// </summary>
        private float sellMultiplier;

        /// <summary>
        /// A sell-back multiplier applied to the price of all gear.
        /// </summary>
        public float SellMultiplier
        {
            get { return sellMultiplier; }
            set { sellMultiplier = value; }
        }


        /// <summary>
        /// The categories of gear in this store.
        /// </summary>
        private List<StoreCategory> storeCategories = new List<StoreCategory>();

        /// <summary>
        /// The categories of gear in this store.
        /// </summary>
        public List<StoreCategory> StoreCategories
        {
            get { return storeCategories; }
            set { storeCategories = value; }
        }
        /// <summary>
        /// The message shown when the party enters the store.
        /// </summary>
        private string welcomeMessage;

        /// <summary>
        /// The message shown when the party enters the store.
        /// </summary>
        public string WelcomeMessage
        {
            get { return welcomeMessage; }
            set { welcomeMessage = value; }
        }
        /// <summary>
        /// The content path and name of the texture for the shopkeeper.
        /// </summary>
        private string shopkeeperTextureName;

        /// <summary>
        /// The content path and name of the texture for the shopkeeper.
        /// </summary>
        public string ShopkeeperTextureName
        {
            get { return shopkeeperTextureName; }
            set { shopkeeperTextureName = value; }
        }


        /// <summary>
        /// The texture for the shopkeeper.
        /// </summary>
        private Texture2D shopkeeperTexture;

        /// <summary>
        /// The texture for the shopkeeper.
        /// </summary>
        [ContentSerializerIgnore]
        public Texture2D ShopkeeperTexture
        {
            get { return shopkeeperTexture; }
        }

    }
}
