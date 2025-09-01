using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.GraphQLApi.Entities
{
    public class BLELink
    {
        public string? Id { get; set; }
        public DateTime? CreationTime { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string? ReceiverVID { get; set; }
        public string? TransmitterVID { get; set; }
        public int? RSSI { get; set; }
        public string? OpVersion { get; set; }
        public bool? Deleted { get; set; }
        public bool? Disabled { get; set; }
    }

    public class BLELinkPayload : SubscriptionPayload<BLELink[]> { }

    public class BLELinksResponse
    {
        public BLELinkPayload Otrackers_getlinksbulk { get; set; }
    }
}
