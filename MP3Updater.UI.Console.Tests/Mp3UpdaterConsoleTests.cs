using CommandLine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MP3Updater.Core;
using MP3Updater.Core.Tests;
using MP3Updater.UI.Console.Host;
using System;
using System.IO;

namespace MP3Updater.UI.Console.Tests
{
    [TestClass]
    public class Mp3UpdaterConsoleTests : FileTestInitialize
    {
        [TestMethod]
        public void Mp3UpdaterDeleteSourcePath()
        {
            var DestinationPath = @"C:\Users\alexejDosdov\source\repos\MP3FileUpdater\MP3Updater.Core.Tests\bin\Debug\netcoreapp3.0\files2";
            Mp3UpdaterConsoleDestinationDirectoryClear mp3UpdaterConsoleDestinationDirectoryClear = new Mp3UpdaterConsoleDestinationDirectoryClear();
            if (new DirectoryInfo(DestinationPath).Exists==false)
            {
                mp3UpdaterConsoleDestinationDirectoryClear.Create(DestinationPath);
                Assert.IsTrue(new DirectoryInfo(DestinationPath).Exists);
            }
            else
            {
                mp3UpdaterConsoleDestinationDirectoryClear.Clear(DestinationPath);
                Assert.IsFalse(new DirectoryInfo(DestinationPath).Exists);
            }
            
         
        }
    }
}
