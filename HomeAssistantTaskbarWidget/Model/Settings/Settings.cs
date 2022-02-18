using System.Collections.Generic;

namespace HomeAssistantTaskbarWidget.Model.Settings
{
    public class Settings
    {
        public string Url { get; set; }
        public string ApiKey { get; set; }
        public int? LogLevel { get; set; } = 2;
        public int? Interval { get; set; } = 30;
        public List<EntityItem> Entities { get; set; } = new List<EntityItem>();
        public string Template { get; set; } = "Define template";
        public Tooltip Tooltip { get; set; }
        public Font Font { get; set; } = new Font();
        public Size Size { get; set; } = new Size();
    }
}