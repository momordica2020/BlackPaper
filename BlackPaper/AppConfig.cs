using System;
using System.IO;
using System.Text.Json;


namespace BlackPaper
{
    public class AppConfig
    {
        public string appName = "BlackPaper";
        public string appSubkey = "";
        public double beginLight = 0.0;
        public bool isAutoChange = true;
        public bool isAutoRun = false;
    }


    public class ConfigManager
    {
        string ConfigFilePath = System.Windows.Forms.Application.StartupPath + "/appconfig.json";

        public AppConfig LoadConfig()
        {
            AppConfig config;
            try
            {
                if (!File.Exists(ConfigFilePath))
                {
                    // 创建默认配置
                    config = new AppConfig();
                    SaveConfig(config); // 保存默认配置到文件
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
