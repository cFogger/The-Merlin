using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using The_Merlin.Models;

namespace The_Merlin.Timer
{
    public static class TimerService
    {
        public static bool IsChronoRunning = false;
        public static TimeSpan ChronoTimer;
        public static TodoItem ActiveTodoSession = null;

        public static void Tick()
        {
            if (IsChronoRunning) ChronoTimer = ChronoTimer.Add(TimeSpan.FromSeconds(1));
        }
    }
}
