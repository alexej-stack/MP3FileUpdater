using Microsoft.VisualStudio.TestTools.UnitTesting;
using MP3Updater.Core;
using MP3Updater.Core.Tests;
using System;

namespace MP3Updater.UI.WPF.Tests
{
    

    [TestClass]
    public class Mp3UpdaterWPFTests :  FileTestInitialize
    {
        [TestMethod]
       public void Mp3UpdaterWPFFilesCount()
        {
            var mp3Directory = new Mp3Directory(SourceDirectory,DestinationDirectory);
            
            mp3Directory.FilesSearchDone += async (sender, args) =>
            {
                var progress = new Progress<int>();
                await mp3Directory.Process(5, progress);
                var filescount = args.FilesCount;
                Assert.IsTrue(filescount>0);
            };
           
        }
    }
}
