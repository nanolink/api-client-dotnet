using ApiClient;
using ApiClient.GraphQLApi.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reactive.Linq;

namespace ApiClientExample
{
    internal class Program
    {
        static readonly string[] apiKeys = { "APIKEY1"};
           


        static TrackerPosition? LookupTrackerOnInstance(string apiKey, string trackerId)
        {
            using var client = new CoreGraphQLClient("https://www.nanolink.com/core05", apiKey);
            var result = client.GetPositions(trackerId /* A specific tracker */);

            foreach (var position in result.ToEnumerable())
            {
                if (position.Data.Otrackers_positionbulk.Type == SyncType.Updated)
                {
                    foreach (var pos in position.Data.Otrackers_positionbulk.Data)
                    {
                        return pos;
                    }
                }
            }
            return null;
        }


        static void Main(string[] args)
        {
            using var client = new CoreGraphQLClient("https://www.nanolink.com/core05", apiKeys[0]); 
            var result = client.GetMeshLinksAll();
            foreach (var link in result.ToEnumerable())
            {
                if (link.Data.Otrackers_getlinksbulk.Type == SyncType.Updated && link.Data.Otrackers_getlinksbulk.Data != null)
                {
                    foreach (var l in link.Data.Otrackers_getlinksbulk.Data)
                    {
                        Console.WriteLine($"{l.LastUpdated} {l.TransmitterVID} -> {l.ReceiverVID} : {l.RSSI}dB");
                    }
                }
            }
        }
    }
}
