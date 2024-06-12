using System;
using YunFetch.Services;
using YunFetch.Helpers;

namespace YunFetch
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO this is ugly and unorganised and lacks commandline arguments, i'll make it pretty one day
            
            // Console.WriteLine("Fetching system information...\n");

            var confController = new ConfController();
            var config = confController.ReadConfig();
            var fetcher = new FetcherService(new CommandExecutor(), confController);
            var formatter = new ColorFormatter(confController);

            var systemInfo = fetcher.GetSystemInfo();
            
            if(config.ClearTermOnRun) Console.Clear();
            
            
            formatter.PrintSystemInfo(systemInfo);
        }
    }
}