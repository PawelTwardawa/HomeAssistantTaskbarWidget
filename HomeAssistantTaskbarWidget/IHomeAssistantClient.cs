using HomeAssistantTaskbarWidget.Model.HA;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeAssistantTaskbarWidget
{
    public interface IHomeAssistantClient
    {
        bool ServerReachable { get; }

        Task<Entity> GetEntityStateAsync(string entity);
        Task<List<Entity>> GetEntitiesStateAsync(IList<string> entities);
        Task<bool> CheckConnection();
    }
}
