using HomeAssistantTaskbarWidget.Model;

namespace HomeAssistantTaskbarWidget
{
    public interface ISettingsManager
    {
        void LoadSettings();
        Settings GetSettings();
        void CheckSettings();
    }
}