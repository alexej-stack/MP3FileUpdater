using System;
using System.Collections.Generic;
using System.Text;

namespace MP3FileUpdater.Core
{
    public class ProgressFiles
    {
        public int ReadedFiles { get; set; }

        public int ReamainingFiles { get; set; }

        public Mp3File mp3File { get; set; }
    }
}
