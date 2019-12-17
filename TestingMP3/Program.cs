using MP3Updater.Core;
using System;
using System.Collections.Concurrent;
using System.IO;

using System.Threading;
using System.Threading.Tasks;

namespace MP3FileUpdateTest
{
    class Program
    {
       static void Main()
        {
            var ss = new MP3File();
            
            ss.GetMP3FileData("D:/Downloads");
            
        }
    }
}
