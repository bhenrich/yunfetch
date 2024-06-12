using System;
using YunFetch.Services;
using YunFetch.Helpers;
using YunFetch.Models;

namespace YunFetch
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Fetching system information...\n");

            var fetcher = new FetcherService(new CommandExecutor());

            var systemInfo = fetcher.GetSystemInfo();

            string textColor = "\x1b[38;5;252m";        // White text
            string highlightColor = "\x1b[38;5;219m";   // Pink highlight
            string accentColor = "\x1b[38;5;117m";      // Cyan accent
            string resetColor = "\x1b[0m";              // Reset

            var formatter = new ColorFormatter(textColor, highlightColor, accentColor, resetColor);
            formatter.PrintSystemInfo(systemInfo);
        }
    }
}