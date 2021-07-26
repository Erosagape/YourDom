using System;
using Microsoft.Xna.Framework.Content;

namespace RPGSample
{
    public class Item : Gear
    {
        /// <summary>
        /// Flags that specify when an item may be used.
        /// </summary>
        public enum ItemUsage
        {
            Combat = 1,
            NonCombat = 2,
        };


        /// <summary>
        /// Description of when the item may be used.
        /// </summary>
        /// <remarks>Defaults to "either", with both values.</remarks>
        private ItemUsage usage = ItemUsage.Combat | ItemUsage.NonCombat;

        /// <summary>
        /// Description of when the item may be used.
        /// </summary>
        /// <remarks>Defaults to "either", with both values.</remarks>
        [ContentSerializer(Optional = true)]
        public ItemUsage Usage
        {
            get { return usage; }
            set { usage = value; }
        }
        /// <summary>
        /// Builds and returns a string describing the power of this item.
        /// </summary>
        public override string GetPowerText()
        {
            return TargetEffectRange.GetModifierString();
        }
        /// <summary>
        /// If true, the statistics change are used as a debuff (subtracted).
        /// Otherwise, the statistics change is used as a buff (added).
        /// </summary>
        private bool isOffensive;

        /// <summary>
        /// If true, the statistics change are used as a debuff (subtracted).
        /// Otherwise, the statistics change is used as a buff (added).
        /// </summary>
        public bool IsOffensive
        {
            get { return isOffensive; }
            set { isOffensive = value; }
        }


        /// <summary>
        /// The duration of the effect of this item on its target, in rounds.
        /// </summary>
        /// <remarks>
        /// If the duration is zero, then the effects last for the rest of the battle.
        /// </remarks>
        private int targetDuration;

        /// <summary>
        /// The duration of the effect of this item on its target, in rounds.
        /// </summary>
        /// <remarks>
        /// If the duration is zero, then the effects last for the rest of the battle.
        /// </remarks>
        public int TargetDuration
        {
            get { return targetDuration; }
            set { targetDuration = value; }
        }


        /// <summary>
        /// The range of statistics effects of this item on its target.
        /// </summary>
        /// <remarks>
        /// This is a debuff if IsOffensive is true, otherwise it's a buff.
        /// </remarks>
        private StatisticsRange targetEffectRange = new StatisticsRange();

        /// <summary>
        /// The range of statistics effects of this item on its target.
        /// </summary>
        /// <remarks>
        /// This is a debuff if IsOffensive is true, otherwise it's a buff.
        /// </remarks>
        public StatisticsRange TargetEffectRange
        {
            get { return targetEffectRange; }
            set { targetEffectRange = value; }
        }


        /// <summary>
        /// The number of simultaneous, adjacent targets affected by this item.
        /// </summary>
        private int adjacentTargets;

        /// <summary>
        /// The number of simultaneous, adjacent targets affected by this item.
        /// </summary>
        public int AdjacentTargets
        {
            get { return adjacentTargets; }
            set { adjacentTargets = value; }
        }
        /// <summary>
        /// The name of the sound effect cue played when the item is used.
        /// </summary>
        private string usingCueName;

        /// <summary>
        /// The name of the sound effect cue played when the item is used.
        /// </summary>
        public string UsingCueName
        {
            get { return usingCueName; }
            set { usingCueName = value; }
        }


        /// <summary>
        /// The name of the sound effect cue played when the item effect is traveling.
        /// </summary>
        private string travelingCueName;

        /// <summary>
        /// The name of the sound effect cue played when the item effect is traveling.
        /// </summary>
        public string TravelingCueName
        {
            get { return travelingCueName; }
            set { travelingCueName = value; }
        }


        /// <summary>
        /// The name of the sound effect cue played when the item affects its target.
        /// </summary>
        private string impactCueName;

        /// <summary>
        /// The name of the sound effect cue played when the item affects its target.
        /// </summary>
        public string ImpactCueName
        {
            get { return impactCueName; }
            set { impactCueName = value; }
        }


        /// <summary>
        /// The name of the sound effect cue played when the item effect is blocked.
        /// </summary>
        private string blockCueName;

        /// <summary>
        /// The name of the sound effect cue played when the item effect is blocked.
        /// </summary>
        public string BlockCueName
        {
            get { return blockCueName; }
            set { blockCueName = value; }
        }
        /// <summary>
        /// An animating sprite used when this item is used.
        /// </summary>
        /// <remarks>
        /// This is optional.  If it is null, then a Using or Creating animation
        /// in SpellSprite is used.
        /// </remarks>
        private AnimatingSprite creationSprite;

        /// <summary>
        /// An animating sprite used when this item is used.
        /// </summary>
        /// <remarks>
        /// This is optional.  If it is null, then a Using or Creating animation
        /// in SpellSprite is used.
        /// </remarks>
        [ContentSerializer(Optional = true)]
        public AnimatingSprite CreationSprite
        {
            get { return creationSprite; }
            set { creationSprite = value; }
        }


        /// <summary>
        /// The animating sprite used when this item is in action.
        /// </summary>
        private AnimatingSprite spellSprite;

        /// <summary>
        /// The animating sprite used when this item is in action.
        /// </summary>
        public AnimatingSprite SpellSprite
        {
            get { return spellSprite; }
            set { spellSprite = value; }
        }


        /// <summary>
        /// The overlay sprite for this item.
        /// </summary>
        private AnimatingSprite overlay;

        /// <summary>
        /// The overlay sprite for this item.
        /// </summary>
        public AnimatingSprite Overlay
        {
            get { return overlay; }
            set { overlay = value; }
        }

    }
}
