using System.Collections.Generic;

namespace RetroFinder.Output
{
    public class SerializedTransposon
    {
        public SerializedLocation Location { get; set; }
        public SerializedFeature[] Features { get; set; }
    }
}
