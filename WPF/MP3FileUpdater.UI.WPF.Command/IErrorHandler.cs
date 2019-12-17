using System;
using System.Collections.Generic;
using System.Text;

namespace MP3FileUpdater.UI.WPF.Command
{
    public interface IErrorHandler
    {
        void HandleError(Exception ex);
    }

}
