using System;

namespace Quickblox.Sdk.Modules.LocationModule.Models
{
    public class GeoRect
    {
        public GeoRect(float latitude1, float longitude1, float latitude2, float longitude2)
        {
            Latitude1 = latitude1;
            Longitude1 = longitude1;

            Latitude2 = latitude2;
            Longitude2 = longitude2;
        }

        public float Latitude1 { get; private set; }
        public float Longitude1 { get; private set; }

        public float Latitude2 { get; private set; }
        public float Longitude2 { get; private set; }

        public override string ToString()
        {
            return String.Format("{0}%3B{1}%3B{2}%3B{3}", Latitude1, Longitude1, Latitude2, Longitude2);
        }
    }
}
