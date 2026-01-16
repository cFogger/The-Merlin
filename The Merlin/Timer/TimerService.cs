using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace The_Merlin.Timer
{
    public static class TimerService
    {
        public static bool IsChronoRunning = false;
        public static TimeSpan ChronoTimer;
        public static int? ActiveSessionId { get; set; }

        public static void Tick()
        {
            if (IsChronoRunning) ChronoTimer = ChronoTimer.Add(TimeSpan.FromSeconds(1));
        }
    }
}
