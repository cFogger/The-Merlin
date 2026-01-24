using System;
using System.Collections.Generic;
using System.Text;
using The_Merlin.Interfaces;

namespace The_Merlin.Services
{
    public class MessageService : IMessageService
    {
        public async Task ShowAsync(string title, string message)
        {
            Page? wind = Application.Current?.Windows[0].Page;
            if (wind != null)
                await wind.DisplayAlertAsync(title, message, "OK");
        }

        public async Task<string> ShowPromptAsync(string title, string message, string placeholder = "")
        {
            Page? wind = Application.Current?.Windows[0].Page;
            if (wind != null)
                return await wind.DisplayPromptAsync(title, message, placeholder: placeholder);
            return "";
        }
    }
}
