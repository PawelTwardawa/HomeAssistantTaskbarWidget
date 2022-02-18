using HomeAssistantTaskbarWidget.Model.HA;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HomeAssistantTaskbarWidget
{
    public class HomeAssistantClient : IHomeAssistantClient
    {
        private readonly string entityStateUrl = "/api/states/{0}";

        private string _url;
        private string _apiKey;
        private ILogger _logger;

        private RestClient _client;

        public bool ServerReachable { get; private set; } = false;

        public HomeAssistantClient(string url, string apiKey, ILogger logger)
        {
            _url = url;
            _apiKey = apiKey;
            _logger = logger;

            var host = url.EndsWith("/") ? url.Remove(url.Length - 1, 1) : url;
            var options = new RestClientOptions(host)
            {
                //ThrowOnAnyError = true,
                Timeout = 2000
            };

            _client = new RestClient(options);
            _client.AddDefaultHeader("Authorization", $"Bearer {apiKey}");
        }

        public async Task<Entity> GetEntityStateAsync(string entity)
        {
            try
            {
                var request = new RestRequest(string.Format(entityStateUrl, entity), Method.Get);
                var response = await _client.GetAsync(request);

                _logger.LogDebug(response.Content);

                if (response.StatusCode != HttpStatusCode.OK)
                    throw new HttpRequestException($"Failed to fetch data. Request return {(int)response.StatusCode} HTTP code");

                if (response.Content == null)
                    throw new NullReferenceException("Response content is null");


                return JsonConvert.DeserializeObject<Entity>(response.Content);
            }
            catch(HttpRequestException ex)
            {
                _logger.LogWarn(ex);
                ServerReachable = false;
                return null;
            }
        }

        public async Task<List<Entity>> GetEntitiesStateAsync(IList<string> entities)
        {
            var result = new List<Entity>();

            foreach (var entity in entities)
            {
                var entityResult = await GetEntityStateAsync(entity);
                if(entityResult != null)
                    result.Add(entityResult);
            }
            return result;
        }

        public async Task<bool> CheckConnection()
        {
            try
            {
                var request = new RestRequest();
                request.Method = Method.Get;
                var response = await _client.GetAsync(request);

                if (response.ErrorException != null)
                {
                    throw response.ErrorException;
                }
            }
            catch(HttpRequestException ex)
            {
                _logger.LogDebug("Cannot find server");
                return ServerReachable = false;
            }

            return ServerReachable = true;
        }
    }
}
