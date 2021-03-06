using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using System.Xml;
using System.Xml.Serialization;
namespace RPGSample
{
    [Serializable]
    public class GameStartDescription
    {

        /// <summary>
        /// The content name of the  map for a new game.
        /// </summary>
        private string mapContentName;

        /// <summary>
        /// The content name of the  map for a new game.
        /// </summary>
        public string MapContentName
        {
            get { return mapContentName; }
            set { mapContentName = value; }
        }
        /// <summary>
        /// The content names of the players in the party from the beginning.
        /// </summary>
        private List<string> playerContentNames = new List<string>();

        /// <summary>
        /// The content names of the players in the party from the beginning.
        /// </summary>
        [XmlArray]
        [XmlArrayItem(ElementName ="Item")]
        public List<string> PlayerContentNames
        {
            get { return playerContentNames; }
            set { playerContentNames = value; }
        }
        /// <summary>
        /// The quest line in action when the game starts.
        /// </summary>
        /// <remarks>The first quest will be started before the world is shown.</remarks>
        private string questLineContentName;

        /// <summary>
        /// The quest line in action when the game starts.
        /// </summary>
        /// <remarks>The first quest will be started before the world is shown.</remarks>
        [ContentSerializer(Optional = true)]
        public string QuestLineContentName
        {
            get { return questLineContentName; }
            set { questLineContentName = value; }
        }
        /// <summary>
        /// Read a GameStartDescription object from the content pipeline.
        /// </summary>
        public class GameStartDescriptionReader : ContentTypeReader<GameStartDescription>
        {
            protected override GameStartDescription Read(ContentReader input,
                GameStartDescription existingInstance)
            {
                GameStartDescription desc = existingInstance;
                if (desc == null)
                {
                    desc = new GameStartDescription();
                }

                desc.MapContentName = input.ReadString();
                desc.PlayerContentNames.AddRange(input.ReadObject<List<string>>());
                desc.QuestLineContentName = input.ReadString();

                return desc;
            }
        }

    }
}
