using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace RPGSample
{
    public class Inn : WorldObject
    {
        /// <summary>
        /// The amount that the party has to pay for each member to stay.
        /// </summary>
        private int chargePerPlayer;

        /// <summary>
        /// The amount that the party has to pay for each member to stay.
        /// </summary>
        public int ChargePerPlayer
        {
            get { return chargePerPlayer; }
            set { chargePerPlayer = value; }
        }
        /// <summary>
        /// The message shown when the player enters the inn.
        /// </summary>
        private string welcomeMessage;

        /// <summary>
        /// The message shown when the player enters the inn.
        /// </summary>
        public string WelcomeMessage
        {
            get { return welcomeMessage; }
            set { welcomeMessage = value; }
        }


        /// <summary>
        /// The message shown when the party successfully pays to stay the night.
        /// </summary>
        private string paidMessage;

        /// <summary>
        /// The message shown when the party successfully pays to stay the night.
        /// </summary>
        public string PaidMessage
        {
            get { return paidMessage; }
            set { paidMessage = value; }
        }


        /// <summary>
        /// The message shown when the party tries to stay but lacks the funds.
        /// </summary>
        private string notEnoughGoldMessage;

        /// <summary>
        /// The message shown when the party tries to stay but lacks the funds.
        /// </summary>
        public string NotEnoughGoldMessage
        {
            get { return notEnoughGoldMessage; }
            set { notEnoughGoldMessage = value; }
        }
        /// <summary>
        /// The content name of the texture for the shopkeeper.
        /// </summary>
        private string shopkeeperTextureName;

        /// <summary>
        /// The content name of the texture for the shopkeeper.
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
