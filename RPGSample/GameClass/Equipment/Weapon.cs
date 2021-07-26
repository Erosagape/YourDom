using System;
namespace RPGSample
{
    public class Weapon : Equipment
    {
        /// <summary>
        /// Builds and returns a string describing the power of this weapon.
        /// </summary>
        public override string GetPowerText()
        {
            return "Weapon Attack: " + TargetDamageRange.ToString();
        }
        /// <summary>
        /// The range of health damage applied by this weapon to its target.
        /// </summary>
        /// <remarks>Damage range values are positive, and will be subtracted.</remarks>
        private Int32Range targetDamageRange;

        /// <summary>
        /// The range of health damage applied by this weapon to its target.
        /// </summary>
        /// <remarks>Damage range values are positive, and will be subtracted.</remarks>
        public Int32Range TargetDamageRange
        {
            get { return targetDamageRange; }
            set { targetDamageRange = value; }
        }
        /// <summary>
        /// The name of the sound effect cue played when the weapon is swung.
        /// </summary>
        private string swingCueName;

        /// <summary>
        /// The name of the sound effect cue played when the weapon is swung.
        /// </summary>
        public string SwingCueName
        {
            get { return swingCueName; }
            set { swingCueName = value; }
        }


        /// <summary>
        /// The name of the sound effect cue played when the weapon hits its target.
        /// </summary>
        private string hitCueName;

        /// <summary>
        /// The name of the sound effect cue played when the weapon hits its target.
        /// </summary>
        public string HitCueName
        {
            get { return hitCueName; }
            set { hitCueName = value; }
        }


        /// <summary>
        /// The name of the sound effect cue played when the weapon is blocked.
        /// </summary>
        private string blockCueName;

        /// <summary>
        /// The name of the sound effect cue played when the weapon is blocked.
        /// </summary>
        public string BlockCueName
        {
            get { return blockCueName; }
            set { blockCueName = value; }
        }
        /// <summary>
        /// The overlay sprite for this weapon.
        /// </summary>
        private AnimatingSprite overlay;

        /// <summary>
        /// The overlay sprite for this weapon.
        /// </summary>
        public AnimatingSprite Overlay
        {
            get { return overlay; }
            set { overlay = value; }
        }

    }
}
