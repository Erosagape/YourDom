using System;
namespace RPGSample
{
    public class WorldEntry<T> : MapEntry<T> where T: ContentObject
    {
        /// <summary>
        /// The name of the map where the content is added.
        /// </summary>
        private string mapContentName;

        /// <summary>
        /// The name of the map where the content is added.
        /// </summary>
        public string MapContentName
        {
            get { return mapContentName; }
            set { mapContentName = value; }
        }
    }
}
