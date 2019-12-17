using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MP3Updater.UI.Console.Host
{
   public class Mp3UpdaterConsoleDestinationDirectoryClear
    {
        public void Clear(string DestinationDirectoryPath)
        {
            if (new DirectoryInfo(DestinationDirectoryPath).Exists)
            {
                new DirectoryInfo(DestinationDirectoryPath).Delete(true);
            }
            

        }
        public void Create(string DestinationDirectoryPath) {
            if (!new DirectoryInfo(DestinationDirectoryPath).Exists)
            {
                new DirectoryInfo(DestinationDirectoryPath).Create();
            }
        }
    }
}
