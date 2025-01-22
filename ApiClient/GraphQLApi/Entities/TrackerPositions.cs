using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.GraphQLApi.Entities
{
    public class TrackerPosition
    {
        public string? TrackerVID { get; set; }
        public DateTime? Stamp { get; set; }
        public string? Source { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public double? Accuracy { get; set; }
        public double? Distance { get; set; }
        public int? RSSI { get; set; }
        public string? SetBy { get; set; }
        public string? OpVersion { get; set; }
    }
    public class TrackerPositionPayload: SubscriptionPayload<TrackerPosition[]> { }

    public class TrackerPositionsResponse
    {
        public TrackerPositionPayload Otrackers_positionbulk { get; set; }
    }
    public class TrackerPositionsResponse2
    {
        public dynamic Otrackers_positionbulk { get; set; }
    }
}
