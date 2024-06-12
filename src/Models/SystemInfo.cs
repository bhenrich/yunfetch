namespace YunFetch.Models
{
    public class SystemInfo
    {
        public string Username { get; set; }
        public string WindowManager { get; set; }
        public string OSVersion { get; set; }
        public string KernelVersion { get; set; }
        public int PackageCount { get; set; }
        public string CurrentTerminal { get; set; }
        public string CurrentShell { get; set; }
        public string Uptime { get; set; }
        public string CPUInfo { get; set; }
        public string GPUInfo { get; set; }
        public string RAMInfo { get; set; }
        public string StorageInfo { get; set; }
        public string MonitorsInfo { get; set; }
    }
}