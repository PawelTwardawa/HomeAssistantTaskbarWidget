using System.Collections.Generic;

namespace HomeAssistantTaskbarWidget.Model
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

    public class EntityItem
    {
        public string Entity { get; set; }
        //public string Template { get; set; } = "Define entity template";
        //public Tooltip Tooltip { get; set; }
    }

    public class Tooltip
    {
        public string Template { get; set; } = "Define tooltip template";
    }

    public class Font
    {
        public int? Size { get; set; } = 9;
        public string Color { get; set; } = "#FFFFFF";
        public string Family { get; set; } = "Arial";
    }

    public class Size
    {
        public int? Height { get; set; } = 40;
        public int? Width { get; set; } = 200;
    }
}
