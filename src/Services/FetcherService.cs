using System;
using YunFetch.Models;
using YunFetch.Helpers;

namespace YunFetch.Services
{
    public class FetcherService
    {
        private readonly ICommandExecutor _commandExecutor;
        private readonly ConfController _confController;

        public FetcherService(ICommandExecutor commandExecutor, ConfController confController)
        {
            _commandExecutor = commandExecutor;
            _confController = confController;
        }

        public SystemInfo GetSystemInfo()
        {
            var config = _confController.ReadConfig();
            // Console.WriteLine("[DBG] " + ExecuteCommand(config.Commands.StorageInfo, config.DefaultErrorMessage));

            return new SystemInfo
            {
                Username = $"{Environment.UserName}@{Environment.UserDomainName}",
                WindowManager = ExecuteCommand(config.Commands.WindowManager, config.DefaultErrorMessage),
                OSVersion = ExecuteCommand(config.Commands.OsVersion, config.DefaultErrorMessage),
                KernelVersion = ExecuteCommand(config.Commands.KernelVersion, config.DefaultErrorMessage),
                PackageCount = GetPackageCount(config.Commands.PackageCount, config.DefaultErrorMessage),
                CurrentTerminal = ExecuteCommand(config.Commands.CurrentTerminal, config.DefaultErrorMessage),
                CurrentShell = ExecuteCommand(config.Commands.CurrentShell, config.DefaultErrorMessage),
                Uptime = ExecuteCommand(config.Commands.Uptime, config.DefaultErrorMessage).Replace("up ", "Uptime ").Trim(),
                CPUInfo = GetCPUInfo(config.Commands.CpuInfo, config.DefaultErrorMessage),
                GPUInfo = GetGPUInfo(config.Commands.GpuInfo, config.DefaultErrorMessage),
                RAMInfo = GetRAMInfo(config.Commands.RamInfo, config.DefaultErrorMessage),
                StorageInfo = GetStorageInfo(config.Commands.StorageInfo, config.DefaultErrorMessage),
                //MonitorsInfo = ExecuteCommand(config.Commands.MonitorsInfo, config.DefaultErrorMessage)
                MonitorsInfo = GetMonitorInfo(config.Commands.MonitorsInfo, config.DefaultErrorMessage)
            };
            
        }

        private string ExecuteCommand(string command, string errorMessage)
        {
            return _commandExecutor.ExecuteBashCommand(command, errorMessage);
        }

        private int GetPackageCount(string command, string errorMessage)
        {
            string output = _commandExecutor.ExecuteBashCommand(command, errorMessage);
            return int.TryParse(output.Trim(), out int packageCount) ? packageCount : 0;
        }

        private string GetMonitorInfo(string command, string errorMessage)
        {
            string output = _commandExecutor.ExecuteBashCommand(command, errorMessage);
            
            // output will have newlines if there's more than 1
            if (output.Contains("\n"))
            {
                string[] lines = output.Split('\n');
                for (int i = 1; i < lines.Length; i++) // skip the first line
                {
                    lines[i] = "  " + lines[i];
                }
                output = string.Join("\n", lines);
            }

            var _lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            string monitorsInfo = "";
            foreach (var line in _lines)
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
        
        private string GetStorageInfo(string command, string errorMessage)
        {
            string output = _commandExecutor.ExecuteBashCommand(command, errorMessage);

            // output will have newlines if there's more than 1
            if (output.Contains("\n"))
            {
                string[] lines = output.Split('\n');
                for (int i = 1; i < lines.Length; i++) // Start from 1 to skip the first line
                {
                    lines[i] = "  " + lines[i];
                }
                output = string.Join("\n", lines);
            }

            string[] _lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            string storageInfo = "";
            foreach (var line in _lines)
            {
                var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 4)
                {
                    storageInfo += $"  {parts[0]}: {parts[2]} used / {parts[1]} total ({parts[3]} available)\n";
                }
            }
            return storageInfo.Trim();
        }
        
        private string GetGPUInfo(string command, string errorMessage)
        {
            string output = _commandExecutor.ExecuteBashCommand(command, errorMessage);
            var lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var parts = line.Split(new[] { ':' }, 2);
                if (parts.Length == 2)
                {
                    return parts[1].Trim().Split("[")[parts[1].Trim().Split("[").Length - 1].Split("]")[0];
                }
            }

            return errorMessage;
            // Line above basically parses the weird lspci model name output. Extra Splitting magic needed for AMD.
        }
        
        private string GetCPUInfo(string command, string errorMessage)
        {
            string output = _commandExecutor.ExecuteBashCommand(command, errorMessage);
            return output.Split("Model name:")[1].Trim();
        }
        
        private string GetRAMInfo(string command, string errorMessage)
        {
            string output = _commandExecutor.ExecuteBashCommand(command, errorMessage);
            var parts = output.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return parts.Length > 2 ? $"{parts[2]} used / {parts[1]} total RAM" : "Unable to fetch RAM info";
        }

    }
}