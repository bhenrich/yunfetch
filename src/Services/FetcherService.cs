using System;
using YunFetch.Models;

namespace YunFetch.Services
{
    public class FetcherService
    {
        private readonly ICommandExecutor _commandExecutor;

        public FetcherService(ICommandExecutor commandExecutor)
        {
            _commandExecutor = commandExecutor;
        }

        public SystemInfo GetSystemInfo()
        {
            return new SystemInfo
            {
                Username = $"{Environment.UserName}@{Environment.UserDomainName}",
                WindowManager = GetWindowManager(),
                OSVersion = GetOSVersion(),
                KernelVersion = GetKernelVersion(),
                PackageCount = GetPackageCount(),
                CurrentTerminal = GetCurrentTerminal(),
                CurrentShell = GetCurrentShell(),
                Uptime = GetUptime(),
                CPUInfo = GetCPUInfo(),
                GPUInfo = GetGPUInfo(),
                RAMInfo = GetRAMInfo(),
                StorageInfo = GetStorageInfo(),
                MonitorsInfo = GetMonitorsInfo()
            };
        }

        private string GetWindowManager()
        {
            return _commandExecutor.ExecuteBashCommand("echo $XDG_CURRENT_DESKTOP", "Unable to fetch window manager");
        }

        private string GetOSVersion()
        {
            try
            {
                string osVersion = System.IO.File.ReadAllText("/etc/os-release");
                foreach (var line in osVersion.Split('\n'))
                {
                    if (line.StartsWith("PRETTY_NAME="))
                    {
                        return line.Split('=')[1].Trim('"');
                    }
                }
            }
            catch (Exception)
            {
                return "Unable to fetch OS version";
            }
            return "Unable to fetch OS version";
        }

        private string GetKernelVersion()
        {
            try
            {
                return System.IO.File.ReadAllText("/proc/version").Split(' ')[2];
            }
            catch (Exception)
            {
                return "Unable to fetch kernel version";
            }
        }

        private int GetPackageCount()
        {
            string output = _commandExecutor.ExecuteBashCommand("pacman -Qq | wc -l", "Unable to fetch package count");
            return int.TryParse(output.Trim(), out int packageCount) ? packageCount : 0;
        }

        private string GetCurrentTerminal()
        {
            return Environment.GetEnvironmentVariable("TERM") ?? "Unknown";
        }

        private string GetCurrentShell()
        {
            return Environment.GetEnvironmentVariable("SHELL") ?? "Unknown";
        }

        private string GetUptime()
        {
            string uptime = _commandExecutor.ExecuteBashCommand("uptime -p", "Unable to fetch uptime");
            return uptime.Replace("up ", "Uptime ").Trim();
        }

        private string GetCPUInfo()
        {
            string output = _commandExecutor.ExecuteBashCommand("lscpu | grep 'Model name:'", "Unable to fetch CPU info");
            return output.Replace("Model name:", "").Trim();
        }

        private string GetGPUInfo()
        {
            string output = _commandExecutor.ExecuteBashCommand("lspci | grep -i 'vga\\|3d\\|2d'", "Unable to fetch GPU info");
            var lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var parts = line.Split(new[] { ':' }, 2);
                if (parts.Length == 2)
                {
                    return parts[1].Trim().Split("[")[parts[1].Trim().Split("[").Length - 1].Split("]")[0];
                }
            }
            return "Unable to fetch GPU info";
        }

        private string GetRAMInfo()
        {
            string output = _commandExecutor.ExecuteBashCommand("free -h | grep 'Mem:'", "Unable to fetch RAM info");
            var parts = output.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return parts.Length > 2 ? $"{parts[2]} used / {parts[1]} total" : "Unable to fetch RAM info";
        }

        private string GetStorageInfo()
        {
            string output = _commandExecutor.ExecuteBashCommand("df -h --output=target,size,used,avail | grep -E '^/'", "Unable to fetch storage info");
            string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            string storageInfo = "";
            foreach (var line in lines)
            {
                var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 4)
                {
                    storageInfo += $"  {parts[0]}: {parts[2]} used / {parts[1]} total ({parts[3]} available)\n";
                }
            }
            return storageInfo.Trim();
        }

        private string GetMonitorsInfo()
        {
            string output = _commandExecutor.ExecuteBashCommand("xrandr --query | grep ' connected'", "Unable to fetch monitors info");
            var lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            string monitorsInfo = "";
            foreach (var line in lines)
            {
                var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 3)
                {
                    var name = parts[0];
                    var resolution = parts[2].Split('+')[0];
                    monitorsInfo += $"  {name} {resolution}\n";
                }
            }
            return monitorsInfo.Trim();
        }
    }
}