using System;
using YunFetch.Models;

namespace YunFetch.Helpers
{
    public class ColorFormatter
    {
        private readonly string _textColor;
        private readonly string _highlightColor;
        private readonly string _accentColor;
        private readonly string _resetColor;

        public ColorFormatter(string textColor, string highlightColor, string accentColor, string resetColor)
        {
            _textColor = textColor;
            _highlightColor = highlightColor;
            _accentColor = accentColor;
            _resetColor = resetColor;
        }

        public void PrintSystemInfo(SystemInfo systemInfo)
        {
            Console.WriteLine($"{_highlightColor}{systemInfo.Username}{_resetColor}");
            Console.WriteLine($"{_accentColor}\uf013 {_textColor}{systemInfo.OSVersion}{_resetColor}");
            Console.WriteLine($"{_accentColor}\uf473 {_textColor}{systemInfo.KernelVersion}\n{_resetColor}");
            Console.WriteLine($"{_accentColor}\ueb29 {_textColor}{systemInfo.PackageCount}{_resetColor}");
            Console.WriteLine($"{_accentColor}\uf488 {_textColor}{systemInfo.WindowManager}{_resetColor}");
            Console.WriteLine($"{_accentColor}\uf120 {_textColor}{systemInfo.CurrentTerminal} - {systemInfo.CurrentShell}{_resetColor}");
            Console.WriteLine($"{_accentColor}\uf017 {_textColor}{systemInfo.Uptime}{_resetColor}");
            Console.WriteLine($"{_accentColor}\uf2db {_textColor}{systemInfo.CPUInfo}{_resetColor}");
            Console.WriteLine($"{_accentColor}\uf2db {_textColor}{systemInfo.GPUInfo}{_resetColor}");
            Console.WriteLine($"{_accentColor}\uf538 {_textColor}{systemInfo.RAMInfo}{_resetColor}");
            Console.WriteLine($"{_accentColor}\uf0a0 {_textColor}{systemInfo.StorageInfo}{_resetColor}");
            Console.WriteLine($"{_accentColor}\uf108 {_textColor}{systemInfo.MonitorsInfo}{_resetColor}");
        }
    }
}