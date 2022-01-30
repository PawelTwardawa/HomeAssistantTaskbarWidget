using HomeAssistantTaskbarWidget.Model;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HomeAssistantTaskbarWidget
{
    public interface IHomeAssistantClient
    {
        Task<Entity> GetEntityStateAsync(string entity);
        Task<List<Entity>> GetEntitiesStateAsync(IList<string> entities);
    }
}
