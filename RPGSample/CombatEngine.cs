using System;
using Microsoft.Xna.Framework;

namespace RPGSample
{
    class CombatEngine
    {
        public static CombatEngine singleton;
        /// <summary>
        /// If true, the combat engine is active and the user is in combat.
        /// </summary>
        public static bool IsActive
        {
            get { return (singleton != null); }
        }
        /// <summary>
        /// Ensure that there is no combat happening right now.
        /// </summary>
        public static void ClearCombat()
        {
            // clear the singleton
            if (singleton != null)
            {
                singleton = null;
            }
        }

        internal static void Update(GameTime gameTime)
        {
            
        }

        internal static void Draw(GameTime gameTime)
        {
            
        }
    }
}
