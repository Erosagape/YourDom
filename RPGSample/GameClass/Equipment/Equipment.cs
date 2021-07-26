using System;
using Microsoft.Xna.Framework.Content;

namespace RPGSample
{
    public class Equipment : Gear
    {
        /// <summary>
        /// The statistics buff applied by this equipment to its owner.
        /// </summary>
        /// <remarks>Buff values are positive, and will be added.</remarks>
        private StatisticsValue ownerBuffStatistics = new StatisticsValue();

        /// <summary>
        /// The statistics buff applied by this equipment to its owner.
        /// </summary>
        /// <remarks>Buff values are positive, and will be added.</remarks>
        [ContentSerializer(Optional = true)]
        public StatisticsValue OwnerBuffStatistics
        {
            get { return ownerBuffStatistics; }
            set { ownerBuffStatistics = value; }
        }
    }
}
