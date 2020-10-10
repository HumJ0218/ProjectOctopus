using System;

namespace WindowsFormsApp
{
    internal static class ConsoleLogger
    {
        private static readonly object Locker = new object();

        public static void Debug(object o)
        {
            lock (Locker)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"[{DateTime.Now:yyyy/MM/dd HH:mm:ss.fff} DD] {o}");
                Console.ResetColor();
            }
        }
        public static void DebugBrighter(object o)
        {
            lock (Locker)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"[{DateTime.Now:yyyy/MM/dd HH:mm:ss.fff} DL] {o}");
                Console.ResetColor();
            }
        }

        public static void Info(object o)
        {
            lock (Locker)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine($"[{DateTime.Now:yyyy/MM/dd HH:mm:ss.fff} ID] {o}");
                Console.ResetColor();
            }
        }
        public static void InfoBrighter(object o)
        {
            lock (Locker)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"[{DateTime.Now:yyyy/MM/dd HH:mm:ss.fff} IL] {o}");
                Console.ResetColor();
            }
        }

        public static void Success(object o)
        {
            lock (Locker)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"[{DateTime.Now:yyyy/MM/dd HH:mm:ss.fff} SD] {o}");
                Console.ResetColor();
            }
        }
        public static void SuccessBrighter(object o)
        {
            lock (Locker)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[{DateTime.Now:yyyy/MM/dd HH:mm:ss.fff} SL] {o}");
                Console.ResetColor();
            }
        }

        public static void Warn(object o)
        {
            lock (Locker)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"[{DateTime.Now:yyyy/MM/dd HH:mm:ss.fff} WD] {o}");
                Console.ResetColor();
            }
        }
        public static void WarnBrighter(object o)
        {
            lock (Locker)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"[{DateTime.Now:yyyy/MM/dd HH:mm:ss.fff} WL] {o}");
                Console.ResetColor();
            }
        }

        public static void Error(object o)
        {
            lock (Locker)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"[{DateTime.Now:yyyy/MM/dd HH:mm:ss.fff} ED] {o}");
                Console.ResetColor();
            }
        }
        public static void ErrorBrighter(object o)
        {
            lock (Locker)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now:yyyy/MM/dd HH:mm:ss.fff} EL] {o}");
                Console.ResetColor();
            }
        }

        public static void Fatal(object o)
        {
            lock (Locker)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine($"[{DateTime.Now:yyyy/MM/dd HH:mm:ss.fff} FD] {o}");
                Console.ResetColor();
            }
        }
        public static void FatalBrighter(object o)
        {
            lock (Locker)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"[{DateTime.Now:yyyy/MM/dd HH:mm:ss.fff} FL] {o}");
                Console.ResetColor();
            }
        }
    }
}
