using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSDeskBand.ContextMenu;
using HomeAssistantTaskbarWidget.Model;
using HomeAssistantTaskbarWidget.Utils;
using HomeAssistantTaskbarWidget.Views;

namespace HomeAssistantTaskbarWidget
{
    [ComVisible(true)]
    [Guid("13248DDF-6DE2-455D-A440-AA7650747987")]
    [CSDeskBand.CSDeskBandRegistration(Name = "HA Taskbar Widget", ShowDeskBand = false)]
    public class HomeAssistantTaskbarWidget : CSDeskBand.CSDeskBandWin
    {
        private readonly string LogFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"HA Taskbar Widget\log.txt");
        private readonly string ConfigFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"HA Taskbar Widget\config.yaml");

        private WidgetUC _control;
        private ILogger _logger;
        private ISettingsManager _settingsManager;
        private ITaskScheduler _taskScheduler;
        private IHomeAssistantClient _homeAssistantClient;

        protected override Control Control => _control;

        public HomeAssistantTaskbarWidget()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var v = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);


            Options.ContextMenuItems = CreateContextMenu();

            _logger = new Logger(LogFilePath, LogLevel.Debug);

            _logger.LogDebug(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            _logger.LogDebug(ConfigFilePath);

            try
            {
                _logger.OnError = exception =>
                {
                    _control.Invoke(new Action(() => _control.UpdateText("Something went wrong. See logs.")));
                };

                _logger.LogInfo($"Application started: {DateTime.Now.ToString()}");

                _control = new WidgetUC(this, _logger);

                Start();
            }
            catch (Exception ex)
            {
                _taskScheduler?.Stop();
                _logger.LogError(ex);
            }
        }

        private void Start()
        {
            _settingsManager = new SettingsManager(ConfigFilePath, _logger);
            _settingsManager.LoadSettings();
            var settings = _settingsManager.GetSettings();
            _settingsManager.CheckSettings();

            _logger.ChangeLogLevel((LogLevel)settings.LogLevel.Value);

            _homeAssistantClient = new HomeAssistantClient(settings.Url, settings.ApiKey, _logger);

            Options.MinHorizontalSize = new CSDeskBand.DeskBandSize(settings.Size.Width.Value, settings.Size.Height.Value);
            _control.UpdateSize(settings.Size);
            _control.UpdateFont(settings.Font);

            _taskScheduler = new TaskScheduler(_logger)
                .SetTaskHandler(TaskHandler)
                .SetInterval(settings.Interval);

            _taskScheduler.Start();
        }

        private void Reload()
        {
            _logger.LogInfo($"Application reloaded: {DateTime.Now.ToString()}");
            _taskScheduler.Stop();

            _settingsManager.LoadSettings();
            _settingsManager.CheckSettings();
            var settings = _settingsManager.GetSettings();

            _logger.ChangeLogLevel((LogLevel)settings.LogLevel.Value); //TODO:
            _homeAssistantClient = new HomeAssistantClient(settings.Url, settings.ApiKey, _logger);
            _control.UpdateFont(settings.Font);
            _taskScheduler.SetInterval(settings.Interval);
            _taskScheduler.Start();    
        }

        private async Task TaskHandler()
        {
            var settings = _settingsManager.GetSettings();

            var entities = settings.Entities.Select(x => x.Entity).ToList();

            var result = await _homeAssistantClient.GetEntitiesStateAsync(entities);

            SetText(settings, result);
            SetTooltip(settings, result);
        }

        private void SetText(Settings settings, List<Entity> entities)
        {
            if (!string.IsNullOrEmpty(settings.Template))
            {
                var template = Helper.ReplaceTemplate(settings.Template, entities);

                _control.Invoke(new Action(() => _control.UpdateText(template)));
            }
            //else if (settings.Entities.Count > 0 && !string.IsNullOrEmpty(settings.Entities[0].Template))
            //{
            //    var template = Helper.ReplaceTemplate(settings.Entities[0].Template, entities[0]);

            //    _control.Invoke(new Action(() => _control.UpdateText(template)));
            //}
            else
            {
                _control.Invoke(new Action(() => _control.UpdateText("Template not defined")));
                _logger.LogError("Template not defined");
            }
        }

        private void SetTooltip(Settings settings, List<Entity> entities)
        {
            //int entityListNumber = 0;

            if (settings.Tooltip != null && !string.IsNullOrEmpty(settings.Tooltip.Template))
            {
                var text = Helper.ReplaceTemplate(settings.Tooltip.Template, entities);

                _control.Invoke(new Action(() => _control.SetTooltip(text)));
            }
            //else if (settings.Entities.Count > 0 &&
            //    settings.Entities[entityListNumber].Tooltip != null &&
            //    !string.IsNullOrEmpty(settings.Entities[entityListNumber].Tooltip.Template))
            //{
            //    var text = Helper.ReplaceTemplate(settings.Entities[entityListNumber].Tooltip.Template, entities[entityListNumber]);

            //    _control.Invoke(new Action(() => _control.SetTooltip(text)));
            //}
        }


        #region ContextMenu
        private List<DeskBandMenuItem> CreateContextMenu()
        {
            //menu schemat
            //
            //HA Taskbar Widget
            //  ⮡ Reload (Action)
            //  ⮡ Open
            //      ⮡ Settings (Action)
            //      ⮡ Logs (Action)

            var reloadSubMenu = new DeskBandMenuAction("Reload");
            reloadSubMenu.Clicked += ReloadSubMenu_Clicked;

            var openSettingsSubMenu = new DeskBandMenuAction("Settings");
            openSettingsSubMenu.Clicked += OpenSettingsSubMenu_Clicked;

            var openLogsSubMenu = new DeskBandMenuAction("Logs");
            openLogsSubMenu.Clicked += OpenLogsSubMenu_Clicked;

            var openSubMenu = new DeskBandMenu("Open");
            openSubMenu.Items.Add(openSettingsSubMenu);
            openSubMenu.Items.Add(openLogsSubMenu);

            var mainMenu = new DeskBandMenu("HA Taskbar Widget");
            mainMenu.Items.Add(reloadSubMenu);
            mainMenu.Items.Add(openSubMenu);

            return new List<DeskBandMenuItem>
            {
                mainMenu
            };
        }

        private void OpenLogsSubMenu_Clicked(object sender, EventArgs e)
        {
             Process.Start(LogFilePath);
        }

        private void OpenSettingsSubMenu_Clicked(object sender, EventArgs e)
        {
            Process.Start(ConfigFilePath);
        }

        private void ReloadSubMenu_Clicked(object sender, EventArgs e)
        {
            Reload();
        }
        #endregion
    }
}
