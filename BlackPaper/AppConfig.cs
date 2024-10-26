using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace BlackPaper
{
    public class AppConfig
    {
        public string appName {  get; set; }
        public string appSubkey {  get; set; }
        public double beginLight { get; set; }
        public bool isAutoChange { get; set; }
        public bool isAutoRun { get; set; }
    }


    public class ConfigManager
    {
        private const string ConfigFilePath = "appconfig.json";

        public AppConfig LoadConfig()
        {
            AppConfig config;
            try
            {
                if (!File.Exists(ConfigFilePath))
                {
                    // 创建默认配置
                    AppConfig defaultConfig = new AppConfig();
                    SaveConfig(defaultConfig); // 保存默认配置到文件
                    return defaultConfig; // 返回默认配置
                }

                // 读取配置文件
                string jsonString = File.ReadAllText(ConfigFilePath);
                config = JsonSerializer.Deserialize<AppConfig>(jsonString);
            }
            catch (Exception ex)
            {
                config = new AppConfig();
            }

            return config;
        }

        public void SaveConfig(AppConfig config)
        {
            string jsonString = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ConfigFilePath, jsonString);
        }

    }

}
