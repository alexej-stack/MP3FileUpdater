using MP3Updater.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MP3FileUpdater.Core
{
   public class Mp3File
    {
        [Key]
        public string Name { get; set; }

        public long Size { get; set; }

        public override string ToString()
        {
            return $"{Size}_{Name}";
        }
        public  string GetNewString(long size,string name)
        {
            return $"{size}_{name}";
        }
     
    }
}
