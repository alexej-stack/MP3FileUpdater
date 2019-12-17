using System;

namespace MP3Updater.Core
{
    public class FileEventArgs:EventArgs
    {
        public int FilesCount { get; set; }

        public FileEventArgs(int filesCount)
        {
            FilesCount = filesCount;
        }
    }
}