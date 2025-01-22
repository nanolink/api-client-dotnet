using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.GraphQLApi.Entities
{
    public enum SyncType
    {
        Deleted,
        Updated,
        Start,
        Done,
        VersionError
    }

    public class GraphQLResult<T>
    {
        public T? Result { get; set; }
        public int GroupVersion { get; set; }
        public GraphQLError[]? Errors { get; set; }
    }

    public class GraphQLError
    {
        public string? StackTrace { get; set; }
        public string? Message { get; set; }
        public string? ErrorKey { get; set; }
        public string? DetailDescription { get; set; }
    }

    public class SubscriptionPayload<T>
    {
        public SyncType Type { get; set; }
        public T? Data { get; set; }
        public int? Total { get; set; }
        public string? DeleteId { get; set; }
        public string? DeleteVersion { get; set; }
    }
}
