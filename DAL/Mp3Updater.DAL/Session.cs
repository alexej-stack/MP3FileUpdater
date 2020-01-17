using System;
using System.Collections.Generic;
using System.Text;

namespace Mp3Updater.DAL
{
   public class Session
    {
        public int Id { get; set; }

        public int ReadedFilesCount { get; set; }

        public int ReamainingFilesCount { get; set; }

        public Mp3FileInfo Mp3File { get; set; }
    }
}
