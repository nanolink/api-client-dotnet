using GraphQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.GraphQLApi.Entities
{
    public class ExternalLoginResponse
    {
        public GraphQLResult<string>? Auth_externallogin { get; set; }
    }
}
