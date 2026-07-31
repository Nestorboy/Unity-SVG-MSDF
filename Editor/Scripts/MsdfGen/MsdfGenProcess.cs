using System.Diagnostics;
using System.IO;

namespace Nessie.MSDF
{
    public class MsdfGenProcess
    {
        private readonly Process _msdfGenProcess = new();

        public MsdfGenProcess()
        {
            StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            StartInfo.CreateNoWindow = true;
            StartInfo.UseShellExecute = false;
            StartInfo.FileName = GetAbsoluteMsdfGenPath();

            _msdfGenProcess.EnableRaisingEvents = true;
        }

        private ProcessStartInfo StartInfo => _msdfGenProcess.StartInfo;

        public MsdfGenArguments Arguments { get; } = new();

        private static string GetLocalMsdfGenPath() => Path.Combine("Packages", "sh.nessie.svg-msdf", "Editor", "bin", "msdfgen.exe");

        private static string GetAbsoluteMsdfGenPath() => Path.GetFullPath(GetLocalMsdfGenPath());

        public void StartAndWaitForExit()
        {
            StartInfo.Arguments = Arguments.ToCommandString();

            _msdfGenProcess.Start();
            _msdfGenProcess.WaitForExit();
        }
    }
}