using System;
using Microsoft.Xna.Framework;

namespace RPGSample
{
    public class Portal : ContentObject
    {
        /// <summary>
        /// The name of the object.
        /// </summary>
        private string name;

        /// <summary>
        /// The name of the object.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        /// <summary>
        /// The map coordinate that the party will automatically walk to 
        /// after spawning on this portal.
        /// </summary>
        private Point landingMapPosition;

        /// <summary>
        /// The map coordinate that the party will automatically walk to 
        /// after spawning on this portal.
        /// </summary>
        public Point LandingMapPosition
        {
            get { return landingMapPosition; }
            set { landingMapPosition = value; }
        }
        /// <summary>
        /// The content name of the map that the portal links to.
        /// </summary>
        private string destinationMapContentName;

        /// <summary>
        /// The content name of the map that the portal links to.
        /// </summary>
        public string DestinationMapContentName
        {
            get { return destinationMapContentName; }
            set { destinationMapContentName = value; }
        }


        /// <summary>
        /// The name of the portal that the party spawns at on the destination map.
        /// </summary>
        private string destinationMapPortalName;

        /// <summary>
        /// The name of the portal that the party spawns at on the destination map.
        /// </summary>
        public string DestinationMapPortalName
        {
            get { return destinationMapPortalName; }
            set { destinationMapPortalName = value; }
        }
    }
}
