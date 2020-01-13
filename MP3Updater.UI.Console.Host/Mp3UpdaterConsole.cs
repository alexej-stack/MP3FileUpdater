using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using Microsoft.Extensions.Logging;
using MP3FileUpdater.Core;
using MP3Updater;
using MP3Updater.Core;
using MP3Updater.Core.Tests;
using MP3Updater.UI.Console.Host;
using NLog;
using ShellProgressBar;

public class Mp3UpdaterConsole
{
    static Logger _logger = LogManager.GetCurrentClassLogger();
    static async Task<int> Main(string[] args)
    {
        
        var resOfParsing = Parser.Default.ParseArguments<ProgramArguments>(args).WithParsed(arguments =>
           {
               try
               {
                   var SourceDirectory = new DirectoryInfo(@arguments.SourceDirectoryPath);
                   
                   _logger.Debug("Source Directory Path Console Application {@SourceDirectory.FullName}", SourceDirectory.FullName);
                   var DestinationDirectory = new DirectoryInfo(@arguments.DestinationDirectoryPath);
                   _logger.Debug("Destination Directory Path Console Application {@DestinationDirectory.FullName}", DestinationDirectory.FullName);
                   var maxThreads = arguments.MaxThreads;
                   _logger.Debug("Threads count {@maxThreads}",maxThreads);
               }
               catch (Exception exeption)
               {
                   _logger.Error(exeption);
                   Console.WriteLine(exeption);

               }
           });
        var progress = new Progress<ProgressFiles>();
        var options = ((Parsed<ProgramArguments>)resOfParsing).Value;


        Mp3UpdaterConsoleDestinationDirectoryClear mp3UpdaterConsoleDestinationDirectoryClear = new Mp3UpdaterConsoleDestinationDirectoryClear();
        mp3UpdaterConsoleDestinationDirectoryClear.Create(options.DestinationDirectoryPath);
        var mp3Directory = new Mp3Directory(new DirectoryInfo(options.SourceDirectoryPath), new DirectoryInfo(options.DestinationDirectoryPath));
        int SourceDirectoryLength = mp3Directory.GetAllFiles(new DirectoryInfo(options.SourceDirectoryPath)).ToList().Count;
        
        Console.Clear();
        progress.ProgressChanged += (sender, progressValue) =>
        {
           
           
            Console.SetCursorPosition(0, 0);
            
            Console.WriteLine("progress {0} from {1}", progressValue.ReadedFilesCount, progressValue.ReamainingFilesCount);
            
        };

    
       
        await mp3Directory.Process(options.MaxThreads, progress);
        mp3UpdaterConsoleDestinationDirectoryClear.Clear(options.DestinationDirectoryPath);
        return 0;
    }

}
