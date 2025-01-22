using ApiClient.GraphQLApi;
using ApiClient.GraphQLApi.Entities;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using System.IdentityModel.Tokens.Jwt;
using System.Reactive.Linq;

namespace ApiClient
{
    public class GraphQLClientBase: IDisposable
    {
        private GraphQLHttpClient client;
        private string _url;
        private string _apiKey;
        private string? _token = null;
        private volatile bool _connected;

        public string Token { get => _apiKey; }

        public GraphQLClientBase(string url, string apiToken)
        {
            _url = $"{url}/api/public";
            _apiKey = apiToken;
            var websocketUrl = _url.Replace("http", "ws") + "/ws";

            client = new GraphQLHttpClient(new GraphQLHttpClientOptions
            {
                WebSocketEndPoint = new Uri(websocketUrl),
                EndPoint = new Uri(_url),               
                ConfigureWebSocketConnectionInitPayload = OnConfigureWebSocketConnectionInitPayload
            }, new NewtonsoftJsonSerializer());
            client.WebsocketConnectionState.Subscribe((c) =>
            {
                switch (c)
                {
                    case GraphQL.Client.Abstractions.Websocket.GraphQLWebsocketConnectionState.Connected:
                        _connected = true;
                        break;
                    case GraphQL.Client.Abstractions.Websocket.GraphQLWebsocketConnectionState.Connecting:
                        _connected = false;
                        break;
                    case GraphQL.Client.Abstractions.Websocket.GraphQLWebsocketConnectionState.Disconnected:
                        _connected = false;
                        break;
                }
            });
        }

        private object? OnConfigureWebSocketConnectionInitPayload(GraphQLHttpClientOptions options)
        {
            return new { authToken = _token };
        }
        public static bool IsJwtTokenValid(string jwtToken)
        {
            try
            {
                // Parse the token
                var handler = new JwtSecurityTokenHandler();

                if (!handler.CanReadToken(jwtToken))
                {
                    Console.WriteLine("Invalid JWT format.");
                    return false;
                }

                var token = handler.ReadJwtToken(jwtToken);

                // Get current UTC time
                var now = DateTimeOffset.UtcNow;

                // Check the `nbf` (Not Before) claim
                if (token.Payload.TryGetValue("nbf", out var nbf))
                {
                    var notBefore = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(nbf));
                    if (now < notBefore)
                    {
                        Console.WriteLine("Token is not valid yet.");
                        return false;
                    }
                }

                // Check the `exp` (Expiration) claim
                if (token.Payload.TryGetValue("exp", out var exp))
                {
                    var expiration = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
                    if (now.AddSeconds(10) >= expiration)
                    {
                        Console.WriteLine("Token has expired.");
                        return false;
                    }
                }

                // If both checks pass, the token is valid in terms of timespan
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error validating token: {ex.Message}");
                return false;
            }
        }

        private async Task<ExternalLoginResponse> Login()
        {
            var retVal = await client.SendQueryAsync<ExternalLoginResponse>(new GraphQLHttpRequest(Queries.ExternalLogin, new { logintoken = _apiKey }));
            if (retVal.Errors != null && retVal.Errors.Length > 0)
            {
                throw new Exception(retVal.Errors[0].Message);
            }
            if (retVal.Data?.Auth_externallogin?.Errors != null && retVal.Data?.Auth_externallogin?.Errors.Length > 0)
            {
                throw new Exception(retVal.Data?.Auth_externallogin?.Errors[0].Message);
            }
#pragma warning disable CS8603 // Possible null reference return.
            return retVal.Data;
#pragma warning restore CS8603 // Possible null reference return.
        }
        public async Task PrepareAsync()
        {
            if (_token != null && IsJwtTokenValid(_token))
            {
                return;
            }
            else
            {
                var loginPayload = await Login();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                _token = loginPayload.Auth_externallogin.Result;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
        }
        public void Prepare() => PrepareAsync().Wait();
        public IObservable<GraphQLResponse<T>> GetMessages<T>(string query, object variables, CancellationToken cancel)
        {
            Prepare();
            if (!_connected)
            {
                CancellationTokenSource timeout = new CancellationTokenSource();
                cancel.Register(() => timeout.Cancel());
                timeout.CancelAfter(TimeSpan.FromSeconds(10));
                client.InitializeWebsocketConnection();
                SpinWait.SpinUntil(() => timeout.IsCancellationRequested || _connected);
            }
            if (cancel.IsCancellationRequested)
            {
                return Observable.Empty<GraphQLResponse<T>>();
            }
            if (!_connected)
            {
                throw new Exception("Service not available");
            }
            return client.CreateSubscriptionStream<T>(new GraphQLRequest(query, variables));
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}
