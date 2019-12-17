using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MP3FileUpdater.UI.WPF.Command
{
   public static class FireAndForgetAsync
    {
        public static async void FireAndForgetSafeAsync(this Task task, IErrorHandler handler = null)
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                handler?.HandleError(ex);
            }
        }
    }
}
