using System;
using System.IO;
using System.Linq;
using HomeAssistantTaskbarWidget.Model.Settings;
using Newtonsoft.Json;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace HomeAssistantTaskbarWidget
{
    public class SettingsManager : ISettingsManager
    {
        private string _path = "config.yaml";

        private ILogger _logger;
        private Settings _settings;

        public SettingsManager(ILogger logger)
        {
            _logger = logger;
        }

        public SettingsManager(string path, ILogger logger)
        {
            _path = path;
            _logger = logger;
        }

        public void LoadSettings()
        {
            var fileContent = File.ReadAllText(_path);

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var result = deserializer.Deserialize<Settings>(fileContent);

            _logger.LogDebug( $"Settings file: \n {fileContent}");
            _logger.LogDebug( $"Settings object: \n {JsonConvert.SerializeObject(result)}");

            _settings = result;
        }

        public Settings GetSettings()
        {
            if (_settings == null)
                LoadSettings();

            return _settings;
        }

        public void CheckSettings()
        {
            if (_settings == null)
                throw new NullReferenceException("Settings not loaded");

            if(string.IsNullOrEmpty(_settings.Url))
                throw new NullReferenceException("Url cannot be null");

            if (string.IsNullOrEmpty(_settings.ApiKey))
                throw new NullReferenceException("ApiKey cannot be null");

            if (_settings.Entities == null || _settings.Entities.Count <= 0)
                throw new NullReferenceException("Cannot be 0 Items");

            if(_settings.Entities.Any(x => string.IsNullOrEmpty(x.Entity)))
                throw new NullReferenceException("Item Entity cannot be null");

            //if (_settings.Entities.Any(x => string.IsNullOrEmpty(x.Template)))
            //    throw new NullReferenceException("Item Template cannot be null");
        }
    }
}
