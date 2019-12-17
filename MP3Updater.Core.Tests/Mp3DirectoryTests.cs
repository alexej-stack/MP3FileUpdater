using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MP3Updater.Core.Tests
{
    [TestClass]
    public class Mp3DirectoryTests : FileTestInitialize
    {
        [TestMethod]
        public void Mp3DirectoryInitialize()
        {
            var sourceDirectoryPath = SourceDirectory;
            var destinationDirectoryPath = DestinationDirectory;

            var mp3Directory = new Mp3Directory(sourceDirectoryPath, destinationDirectoryPath);

            Assert.IsNotNull(mp3Directory);

        }

        [TestMethod]
        public void Mp3Directory_PropertiesInitialized()
        {
            var sourceDirectoryPath = SourceDirectory;
            var destinationDirectoryPath = DestinationDirectory;

            var mp3Directory = new Mp3Directory(sourceDirectoryPath, destinationDirectoryPath);

            var destination = mp3Directory.DestinationDirectoryPath;
            var source = mp3Directory.SourceDirectoryPath;

            Assert.AreEqual(sourceDirectoryPath, source);
            Assert.AreEqual(destinationDirectoryPath, destination);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Mp3DirectoryInitialize_SourceIsNull()
        {
            DirectoryInfo sourceDirectoryPath = null;
            var destinationDirectoryPath = DestinationDirectory;

            var mp3Directory = new Mp3Directory(sourceDirectoryPath, destinationDirectoryPath);

            Assert.IsNull(mp3Directory);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Mp3DirectoryInitialize_DestinationIsNull()
        {
            var sourceDirectoryPath = SourceDirectory;
            DirectoryInfo destinationDirectoryPath = null;
           
            var mp3Directory = new Mp3Directory(sourceDirectoryPath, destinationDirectoryPath);
            

            Assert.IsNull(mp3Directory);
        }

        private static object _lockObject = new object();

     
        [TestMethod]
        public async Task Mp3DirectoryStartProcessTest()
        {
            var maxThreads = 5;
            var progress = new Progress<int>();
            var progressPercentage = 0;

            var autoResetEvent = new AutoResetEvent(false);
            var timeout = TimeSpan.FromSeconds(5);

            progress.ProgressChanged += (sender, progressValue) =>
            {
                lock (_lockObject)
                {
                    progressPercentage = progressValue;
                }

                autoResetEvent.Set();
            };
            var mp3Directory = new Mp3Directory(SourceDirectory, DestinationDirectory);

            await mp3Directory.Process(maxThreads, progress);

            if (autoResetEvent.WaitOne(timeout))
            {
                Assert.IsTrue(progressPercentage > 0);
            }
            else
            {
                Assert.Fail("Timeout expired");
            }

        }
        [TestMethod]
        public async Task Mp3DirectoryChekNumRenamedFiles()
        {
            var mp3Directory = new Mp3Directory(SourceDirectory, DestinationDirectory);
            var maxThreads = 2;
            var progress = new Progress<int>();
            await mp3Directory.Process(maxThreads, progress);
            var sourceDirectorySize = mp3Directory.GetAllFiles(SourceDirectory).ToArray().Length;
            var destinationDirectorySize = mp3Directory.GetAllFiles(DestinationDirectory).ToArray().Length;
            var procentOfRename = sourceDirectorySize / destinationDirectorySize;

            Assert.IsTrue(procentOfRename == 1);

        }

        [TestMethod]
        public async Task Mp3DirectoryCheckOfCounterValue()
        {
            var mp3Directory = new Mp3Directory(SourceDirectory, DestinationDirectory);
            var maxThreads = 2;
            var progress = new Progress<int>();
            var sourceDirectorySize = SourceDirectory.GetFiles().Length;
            await mp3Directory.Process(maxThreads, progress);
            var Mp3Counter = mp3Directory.counter;
            Assert.IsTrue(Mp3Counter > 0);

        }
        [TestMethod]
        public void Mp3DirectoryGetAllFilesCheck()
        {
            var expectedCount = 350*4+40;
            var mp3Directory = new Mp3Directory(SourceDirectory, DestinationDirectory);
            var files = mp3Directory.GetAllFiles(SourceDirectory);
            var convertedFiles = files.ToList();
            var existedCount = convertedFiles.Count;
            Assert.IsTrue(expectedCount == existedCount);
        }
        [TestMethod]
        
        public void Mp3DirectotySourceDirectoryFilesExist()
        {
            var mp3Directory = new Mp3Directory(SourceDirectory, DestinationDirectory);
            var files = mp3Directory.GetAllFiles(SourceDirectory);
            Assert.IsTrue(files.ToList().Count>0);
        }
        [TestMethod]
        
        public void Mp3DirectotyFileTypeCheck()
        {
            var mask = ".mp3";
            var mp3Directory = new Mp3Directory(SourceDirectory, DestinationDirectory);
            var files = mp3Directory.GetAllFiles(SourceDirectory).ToList();
            foreach (var file in files)
            {
                var t = Path.GetExtension(file.Name);
                Assert.IsTrue(Path.GetExtension(file.Name)== mask);
            }
            
        }
        [TestMethod]
        public void Mp3DirectoryFilesCount()
        {
            int FilesCount=0;
            var mp3Directory = new Mp3Directory(SourceDirectory, DestinationDirectory);
            mp3Directory.FilesSearchDone += (sender, args) =>
            {
               
                var x = args.FilesCount;

            };
            Assert.IsTrue(FilesCount==0);
        }
    }
}
