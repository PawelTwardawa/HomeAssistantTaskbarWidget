using System;

namespace HomeAssistantTaskbarWidget.Model.HA
{
    public class Entity
    {
        public string entity_id { get; set; }
        public State state { get; set; }
        public Attributes attributes { get; set; }
        public DateTime last_changed { get; set; }
        public DateTime last_updated { get; set; }
        public Context context { get; set; }
    }
}
