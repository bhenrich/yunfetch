using System.Diagnostics;

namespace YunFetch.Services
{
    public interface ICommandExecutor
    {
        string ExecuteBashCommand(string command, string errorMessage);
    }

    public class CommandExecutor : ICommandExecutor
    {
        public string ExecuteBashCommand(string command, string errorMessage)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "bash";
                process.StartInfo.Arguments = $"-c \"{command}\"";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return output.Trim();
            }
            catch (Exception)
            {
                return errorMessage;
            }
        }
    }
}