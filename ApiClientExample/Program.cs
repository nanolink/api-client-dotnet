using ApiClient;
using ApiClient.GraphQLApi.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reactive.Linq;

namespace ApiClientExample
{
    internal class Program
    {
        static readonly string[] apiKeys = { "APIKEY1", "APIKEY2", "APIKEY3" };
           


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
            foreach (var apiKey in apiKeys)
            {
                var result = LookupTrackerOnInstance(apiKey, "E63280111D7C");
                if (result != null)
                {
                    Console.WriteLine(JsonConvert.SerializeObject(result));
                    break;
                }
            }
        }
    }
}
