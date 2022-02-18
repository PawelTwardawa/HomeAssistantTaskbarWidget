using System.Collections.Generic;

namespace HomeAssistantTaskbarWidget.Model.Settings
{
    public class EntityItem
    {
        public string Entity { get; set; }
        //public string Template { get; set; } = "Define entity template";
        //public Tooltip Tooltip { get; set; }

        public IDictionary<string, string> Mapping { get; set; }
    }
}
