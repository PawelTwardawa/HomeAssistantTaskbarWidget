using HomeAssistantTaskbarWidget.Model.Settings;

namespace HomeAssistantTaskbarWidget
{
    public interface ISettingsManager
    {
        void LoadSettings();
        Settings GetSettings();
        void CheckSettings();
    }
}