using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace StatuxGUI.Services
{
    public class AppSettingsManager
    {
        private static AppSettingsManager _instance;
        private JObject _settings;

        private const string Namespace = "StatuxGUI";
        private const string Filename = "appsettings.json";

        public AppSettingsManager()
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(AppSettingsManager)).Assembly;
            var stream = assembly.GetManifestResourceStream($"{Namespace}.{Filename}");
            using(var reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                _settings = JObject.Parse(json);
            }
        }

        public static AppSettingsManager Settings
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new AppSettingsManager();   
                }

                return _instance;
            }
        }

        public string this[string name]
        {
            get
            {
                try
                {
                    var path = name.Split(':');

                    JToken node = _settings[path[0]];
                    for(int index = 1; index < path.Length; index++)
                    {
                        node = _settings[path[index]];
                    }

                    return node.ToString();
                }
                catch (Exception)
                {
                    Debug.WriteLine($"Unable to retrieve setting '{name}'");
                    return string.Empty;
                }
            }
        }
    }
}
