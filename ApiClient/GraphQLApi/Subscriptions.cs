using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.GraphQLApi
{
    public static class Subscriptions
    {
        public const string TrackerPositions =
@"subscription positions($opversion: String, $trackers: [String!]) {
  otrackers_positionbulk(
    subscribe: false
    opversion: $opversion
    trackers: $trackers
  ) {
    type
    total
    deleteId
    data {
      trackerVID
      stamp
      source
      longitude
      latitude
      accuracy
      distance
      rSSI
      setBy
      opVersion
    }
    deleteVersion
  }
}";
    }
}
