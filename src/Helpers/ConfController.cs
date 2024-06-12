using System;
using System.IO;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace YunFetch.Helpers
{
    public class ConfController
    {
        private static string username = Environment.UserName;
        private static string ConfigFilePath = $"/home/{username}/.config/yunfetch/conf.yaml";

        public ConfController()
        {
            // Ensure config directory exists
            var configDir = Path.GetDirectoryName(ConfigFilePath);
            if (!Directory.Exists(configDir))
            {
                Directory.CreateDirectory(configDir);
            }

            // Check if config file exists, if not, generate a default one
            if (!File.Exists(ConfigFilePath))
            {
                GenerateDefaultConfig();
            }
        }

        public Config ReadConfig()
        {
            try
            {
                using (var reader = new StreamReader(ConfigFilePath))
                {
                    var deserializer = new DeserializerBuilder()
                        .WithNamingConvention(CamelCaseNamingConvention.Instance)
                        .Build();

                    return deserializer.Deserialize<Config>(reader);
                }
            }
            catch (Exception)
            {
                throw new Exception("Error reading config file.");
            }
        }

        private void GenerateDefaultConfig()
        {
            try
            {
                var config = new Config
                {
                    Colors = new Colors
                    {
                        TextColor = "\x1b[38;5;252m",
                        HighlightColor = "\x1b[38;5;219m",
                        AccentColor = "\x1b[38;5;117m",
                        ResetColor = "\x1b[0m"
                    },
                    DefaultUsername = "defaultuser",
                    DefaultErrorMessage = "Unable to fetch information",
                    ClearTermOnRun = true,
                    Commands = new Commands
                    {
                        WindowManager = "echo $XDG_CURRENT_DESKTOP",
                        OsVersion = "cat /etc/os-release | grep PRETTY_NAME | cut -d= -f2 | tr -d '\"'",
                        KernelVersion = "uname -r",
                        PackageCount = "pacman -Qq | wc -l",
                        CurrentTerminal = "echo $TERM",
                        CurrentShell = "echo $SHELL",
                        Uptime = "uptime -p",
                        CpuInfo = "lscpu | grep 'Model name:'",
                        GpuInfo = "lspci | grep -i 'vga\\|3d\\|2d'",
                        RamInfo = "free -h | grep 'Mem:'",
                        StorageInfo = "df -h --output=target,size,used,avail | grep -E '^/'",
                        MonitorsInfo = "xrandr --query | grep ' connected'"
                    }
                };

                var serializer = new SerializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();

                var yaml = serializer.Serialize(config);

                File.WriteAllText(ConfigFilePath, yaml, Encoding.UTF8);
            }
            catch (Exception)
            {
                throw new Exception("Error generating default config file.");
            }
        }
    }

    public class Config
    {
        public Colors Colors { get; set; }
        public string DefaultUsername { get; set; }
        public string DefaultErrorMessage { get; set; }
        public bool ClearTermOnRun { get; set; }
        public Commands Commands { get; set; }
    }

    public class Colors
    {
        public string TextColor { get; set; }
        public string HighlightColor { get; set; }
        public string AccentColor { get; set; }
        public string ResetColor { get; set; }
    }

    public class Commands
    {
        public string WindowManager { get; set; }
        public string OsVersion { get; set; }
        public string KernelVersion { get; set; }
        public string PackageCount { get; set; }
        public string CurrentTerminal { get; set; }
        public string CurrentShell { get; set; }
        public string Uptime { get; set; }
        public string CpuInfo { get; set; }
        public string GpuInfo { get; set; }
        public string RamInfo { get; set; }
        public string StorageInfo { get; set; }
        public string MonitorsInfo { get; set; }
    }
}