using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MP3Updater.Core.Tests
{
    [TestClass]
    public class FileTestInitialize
    {

        protected DirectoryInfo SourceDirectory;
        protected DirectoryInfo SourceSubDirectory1;
        protected DirectoryInfo SourceSubDirectory2;
        protected DirectoryInfo SourceSubDirectory3;

        protected DirectoryInfo DestinationDirectory;

        private async Task CreateFiles(string fileExtension, int filesCount, string directoryName)
        {
            for (var fileIndex = 0; fileIndex < filesCount; fileIndex++)
            {
                var random = new Random();
                var buffer = new byte[10000];
                random.NextBytes(buffer);

                var fileName = Path.Combine(directoryName, $"{Guid.NewGuid().ToString()}.{fileExtension}");
                if (!File.Exists(fileName))
                {
                    using (var fileStream = File.Create(fileName))
                    {
                       await fileStream.WriteAsync(buffer);
                    }
                }

            }
        }

        [TestInitialize]
        public async Task Initialize()
        {
            var sourceDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(),"files");
            var sourceSubDirectoryPath1 = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "files"), "files1");
            var sourceSubDirectoryPath2 = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "files"), "files2");
            var sourceSubDirectoryPath3 = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "files"), "files3");
            var destinationDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "files2");
          
             SourceDirectory = new DirectoryInfo(sourceDirectoryPath);
             SourceSubDirectory1 = new DirectoryInfo(sourceSubDirectoryPath1);
             SourceSubDirectory2 = new DirectoryInfo(sourceSubDirectoryPath2);
             SourceSubDirectory3 = new DirectoryInfo(sourceSubDirectoryPath3);

            if (!SourceDirectory.Exists||!SourceSubDirectory1.Exists || !SourceSubDirectory2.Exists|| !SourceSubDirectory3.Exists)
            {
                SourceDirectory.Create();
                SourceSubDirectory1.Create();
                SourceSubDirectory2.Create();
                SourceSubDirectory3.Create();
            }

            var tasksCount = 5;
            var tasks = new List<Task>();
            for (var taskIndex = 0; taskIndex < tasksCount; taskIndex++)
            {
                tasks.Add(CreateFiles("mp3", 2, SourceDirectory.FullName));
                tasks.Add(CreateFiles("mp3", 2, SourceSubDirectory1.FullName));
                tasks.Add(CreateFiles("mp3", 2, SourceSubDirectory2.FullName));
                tasks.Add(CreateFiles("mp3", 2, SourceSubDirectory3.FullName));
            }

            await Task.WhenAll(tasks);
            
            DestinationDirectory = new DirectoryInfo(destinationDirectoryPath);
            if (!DestinationDirectory.Exists)
            {
                DestinationDirectory.Create();
            }
        }

        [TestCleanup]
        public void DeInitialize()
        {// отчистить DestinationDirectory
            var destinationDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "files2");
            //var newpath = Directory.GetCurrentDirectory();
            DirectoryInfo DestinationDirectory = new DirectoryInfo(destinationDirectoryPath);
            if (DestinationDirectory.Exists)
            {
                DestinationDirectory.Delete(true);
            }
            //var sourceDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "files");
            ////var newpath = Directory.GetCurrentDirectory();
            //DirectoryInfo SourceDirectory = new DirectoryInfo(sourceDirectoryPath);
            //if (SourceDirectory.Exists)
            //{
            //    SourceDirectory.Delete(true);
            //}

            //Assert.IsNull(DestinationDirectory);
        }
    }
}
