using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CommandLine;

namespace MP3Updater.UI.Console.Host
{
    public class ProgramArguments
    {
        [Option('a',"SourceDirectory",HelpText ="Source Directory name ")]
        public string SourceDirectoryPath { get; set; }

        [Option('b',"DestinationDirectory",HelpText="Ddestination Directory path")]
        public string DestinationDirectoryPath { get; set; }
        [Option('m',"MaxThreads")]
        public int MaxThreads { get; set; }
    }
}
