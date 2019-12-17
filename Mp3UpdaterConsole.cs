using System;
using MP3Updater.Core;

public class Class1
{
    static void Main(string[] args)

    {

        var mp3Directory = new Mp3Directory(SourceDirectory, DestinationDirectory);
        var maxThreads = 2;
        var progress = new Progress<int>();
        await mp3Directory.Process(maxThreads, progress);
        var Mp3Counter=mp3Directory.counter ;
        Console.WriteLine(Mp3Counter);

    }
}
