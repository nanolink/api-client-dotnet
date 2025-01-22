using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.GraphQLApi
{
    public static class Queries
    {
        public const string ExternalLogin =
@"query login($logintoken: String!) {
  auth_externallogin(logintoken: $logintoken) {
    result
    groupVersion
    errors {
      stackTrace
      message
      errorKey
      detailDescription
    }
  }
}";     
    }
}
