using System;
namespace RPGSample
{
    public class WeightedContentEntry<T> : ContentEntry<T> where T :ContentObject
    {
        /// <summary>
        /// The weight of this content within the group, for statistical distribution.
        /// </summary>
        private int weight;

        /// <summary>
        /// The weight of this content within the group, for statistical distribution.
        /// </summary>
        public int Weight
        {
            get { return weight; }
            set { weight = value; }
        }
    }
}
