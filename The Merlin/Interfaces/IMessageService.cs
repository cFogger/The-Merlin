using System;
using System.Collections.Generic;
using System.Text;

namespace The_Merlin.Interfaces
{
    public interface IMessageService
    {
        Task ShowAsync(string title, string message);
    }
}
