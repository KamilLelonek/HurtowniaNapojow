using System.Diagnostics;
using System.Windows;

namespace HurtowniaNapojow.Utils
{
    public static class Timer
    {
        private static readonly Stopwatch Stopwatch = new Stopwatch();

        public static void StartTimer()
        {
            Stopwatch.Reset();
            Stopwatch.Start();
        }

        public static void StopTimer()
        {
            Stopwatch.Stop();
            var milliseconds = Stopwatch.ElapsedMilliseconds.ToString();
            MessageBox.Show("Elapsed time in milliseconds is " + milliseconds);
        }
    }
}