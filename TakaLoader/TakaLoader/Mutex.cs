using System;
using System.Threading;
using TakaLoader;

namespace TakaLoader
{
    internal sealed class MutexControl
    {
        // Prevent the program from running twice
        private static Mutex _mutex;

        public static void Check()
        {
            var mutexCreated = false;
            _mutex = new Mutex(true, Config.Mutex, out mutexCreated);
            switch (mutexCreated)
            {
                case true:
                    _mutex.ReleaseMutex();
                    break;
                case false:
                    //App is already running, close this!
                    Environment.Exit(0);
                    break;
            }
        }
    }
}