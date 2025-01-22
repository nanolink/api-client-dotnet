using ApiClient.GraphQLApi;
using ApiClient.GraphQLApi.Entities;
using GraphQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient
{
    public class CoreGraphQLClient : GraphQLClientBase
    {
        public CoreGraphQLClient(string url, string apiToken) : base(url, apiToken)
        {
        }

        public IObservable<GraphQLResponse<TrackerPositionsResponse>> GetPositions(params string[] trackers)
        {
            string? opversion = null;
            if (trackers.Length == 0)
            {
                trackers = null;
            }
            return GetMessages<TrackerPositionsResponse>(Subscriptions.TrackerPositions, new { trackers,  opversion }, CancellationToken.None);
        }
    }
}
