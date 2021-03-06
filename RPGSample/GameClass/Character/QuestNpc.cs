using System;
using Microsoft.Xna.Framework.Content;

namespace RPGSample
{
    public class QuestNpc : Character
    {
        /// <summary>
        /// The dialogue that the Npc says when it is greeted in the world.
        /// </summary>
        private string introductionDialogue;

        /// <summary>
        /// The dialogue that the Npc says when it is greeted in the world.
        /// </summary>
        public string IntroductionDialogue
        {
            get { return introductionDialogue; }
            set { introductionDialogue = value; }
        }
        /// <summary>
        /// Read a QuestNpc object from the content pipeline.
        /// </summary>
        public class QuestNpcReader : ContentTypeReader<QuestNpc>
        {
            protected override QuestNpc Read(ContentReader input,
                QuestNpc existingInstance)
            {
                QuestNpc questNpc = existingInstance;
                if (questNpc == null)
                {
                    questNpc = new QuestNpc();
                }

                input.ReadRawObject<Character>(questNpc as Character);

                questNpc.IntroductionDialogue = input.ReadString();

                return questNpc;
            }
        }
    }
}
