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
        public const string MeshBLELinks =
@"subscription getlinks($transmitterId: String, $opversion: String) {
  otrackers_getlinksbulk(
    receiverTypes: [MESH_GATE_TRACKER]
    transmitterVIDs: [$transmiterId]
    subscribe: true
    opversion: $opversion
    subscriberssiupdates: true
  ) {
    type
    total
    deleteId
    data {
      id
      creationTime
      lastUpdated
      receiverVID
      transmitterVID
      rSSI
      opVersion
      deleted
      disabled
    }
    deleteVersion
  }
}";
        public const string MeshBLELinksAll =
@"subscription getlinks($opversion: String) {
  otrackers_getlinksbulk(
    receiverTypes: [MESH_GATE_TRACKER]
    subscribe: true
    opversion: $opversion
    subscriberssiupdates: true
  ) {
    type
    total
    deleteId
    data {
      id
      creationTime
      lastUpdated
      receiverVID
      transmitterVID
      rSSI
      opVersion
      deleted
      disabled
    }
    deleteVersion
  }
}";
    }
}
