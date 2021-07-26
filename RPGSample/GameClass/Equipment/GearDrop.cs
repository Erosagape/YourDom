using System;
namespace RPGSample
{
    public class GearDrop
    {
        /// <summary>
        /// The content name of the gear.
        /// </summary>
        private string gearName;

        /// <summary>
        /// The content name of the gear.
        /// </summary>
        public string GearName
        {
            get { return gearName; }
            set { gearName = value; }
        }


        /// <summary>
        /// The percentage chance that the gear will drop, from 0 to 100.
        /// </summary>
        private int dropPercentage;

        /// <summary>
        /// The percentage chance that the gear will drop, from 0 to 100.
        /// </summary>
        public int DropPercentage
        {
            get { return dropPercentage; }
            set { dropPercentage = (value > 100 ? 100 : (value < 0 ? 0 : value)); }
        }

    }
}
