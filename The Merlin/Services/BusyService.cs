using System;
using System.Collections.Generic;
using System.Text;

namespace The_Merlin.Services
{
    public class BusyService
    {
        private int _count;

        public bool IsBusy => _count > 0;

        public event Action<bool> BusyChanged;

        public void Begin()
        {
            _count++;
            BusyChanged?.Invoke(IsBusy);
        }

        public void End()
        {
            if (_count > 0)
                _count--;

            BusyChanged?.Invoke(IsBusy);
        }
    }
}
