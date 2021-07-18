using System;
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
    }
}
