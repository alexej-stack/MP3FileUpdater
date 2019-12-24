using MP3FileUpdater.Core;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MP3Updater.Core
{
    public class Mp3Directory
    {
        private const string mp3Mask = "*.mp3";
        static Logger _logger = LogManager.GetCurrentClassLogger();
        public DirectoryInfo SourceDirectoryPath { get; }
        public DirectoryInfo DestinationDirectoryPath { get; }
        CancellationTokenSource source = new CancellationTokenSource();
        public int counter;
        public int sourceCounter;
        private BlockingCollection<FileInfo> _collectionFiles;
        private static object _lockObject = new object();

        /// <summary>
        /// Occurs when [files search done].
        /// </summary>
        public event EventHandler<FileEventArgs> FilesSearchDone;

        /// <summary>
        /// Initializes a new instance of the <see cref="Mp3Directory"/> class.
        /// </summary>
        /// <param name="sourceDirectoryPath">The source directory path.</param>
        /// <param name="destinationDirectoryPath">The destination directory path.</param>
        /// <exception cref="System.ArgumentNullException">
        /// sourceDirectoryPath
        /// or
        /// destinationDirectoryPath
        /// </exception>
        public Mp3Directory(DirectoryInfo sourceDirectoryPath, DirectoryInfo destinationDirectoryPath)
        {
            SourceDirectoryPath = sourceDirectoryPath ?? throw new System.ArgumentNullException(nameof(sourceDirectoryPath));
            _logger.Debug("SourcePath {@sourceDirectoryPath}", SourceDirectoryPath);
            DestinationDirectoryPath = destinationDirectoryPath ?? throw new System.ArgumentNullException(nameof(destinationDirectoryPath));
            _collectionFiles = new BlockingCollection<FileInfo>();
            _logger.Debug("DestinationPath {@destinationDirectoryPath}", DestinationDirectoryPath);
        }

        /// <summary>
        /// File fatching method
        /// </summary>
        public void Producer()
        {
            _logger.Trace("Start producer working");
            foreach (var fileinfo in GetAllFiles(SourceDirectoryPath))
            {
                _collectionFiles.Add(fileinfo);

                sourceCounter++;
            }

            _collectionFiles.CompleteAdding();
            _logger.Debug("file fatching count {@sourceCounter}", sourceCounter);
            _logger.Trace("Stop Producer working");
        }


        /// <summary>
        /// Gets all files.
        /// </summary>
        /// <param name="directoryInfo">The directory information.</param>
        /// <remarks>fetch all files from all directories and subdirectories from the path</remarks>
        /// <returns>IEnumerable files collection</returns>
        public IEnumerable<FileInfo> GetAllFiles(DirectoryInfo directoryInfo)
        {
            var files = directoryInfo.EnumerateFiles(mp3Mask);
            // if (files == null) yield break;
            foreach (var file in files)
            {
                yield return file;
            }

            var directories = directoryInfo.EnumerateDirectories();
            foreach (var innerDirectory in directories)
            {
                var innerFiles = GetAllFiles(innerDirectory);
                foreach (var fileinfo in innerFiles)
                {
                    yield return fileinfo;
                }

            }


        }

        /// <summary>
        /// Stops the writing process.
        /// </summary>
        public async Task StopProcess()
        {
            source.Cancel();
            await Task.CompletedTask;
        }

        /// <summary>
        /// Consumers the specified progress.
        /// </summary>
        /// <param name="progress">The progress value.</param>
        /// <param name="token">The cencel token.</param>
        /// <
        public async Task Consumer(IProgress<int> progress, CancellationToken token)
        {

            _logger.Trace("Start Consumer working");
            await Task.Run(() =>
                {
                    while (!_collectionFiles.IsCompleted && !token.IsCancellationRequested)
                    {


                        if (_collectionFiles.TryTake(out var fileInfo))
                        {
                            SaveFile(fileInfo.FullName, Path.Combine(DestinationDirectoryPath.FullName, GetNewFileName(fileInfo)));
                            FileEventArgs args = new FileEventArgs(counter);
                            OnFilesSearchDone(this, args);
                            counter++;
                            progress.Report(counter);

                            _logger.Debug("Renamed files count {@counter}", counter);

                        }

                    }
                });
            _logger.Trace("Stop Consumr working");

        }
        public IEnumerable<string> GetFileName()
        {
            while (!_collectionFiles.IsCompleted )
            {
                if (_collectionFiles.TryTake(out var fileInfo))
                {
                    var name = GetNewFileName(fileInfo);
                    yield return name;

                }
                else
                {
                    yield return "end";
                }
            }

        }
        /// <summary>
        /// Rename all files from all directories and subdirectories from path. Format is size_oldname.mp3 .
        /// </summary>
        /// <param name="maxThreads">The maximum threads.</param>
        /// <param name="progress">The progress.</param>
        /// <remarks>Rename all files from all directories and subdirectories from path. Format is size_oldname </remarks>
        public async Task Process(int maxThreads, IProgress<int> progress)
        {
            _logger.Trace("Start Procces working");
            CancellationToken token = source.Token;
            _logger.Debug("Procces status {@token}", token);
            var producerTask = new Task(Producer, token);

            List<Task> tasks = new List<Task>(maxThreads);

            try
            {
                producerTask.Start();
                tasks.Add(producerTask);

                for (int taskIndex = 0; taskIndex < maxThreads; taskIndex++)
                {
                    tasks.Add(Consumer(progress, token));
                }
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                Console.WriteLine(ex);
            }
            finally
            {

                producerTask.Dispose();
                _logger.Info("Clear oll threads");
            }
            _logger.Trace("Stop Procces working");

        }

        private void OnFilesSearchDone(object sender, FileEventArgs args)
        {

            FilesSearchDone?.Invoke(sender, args);
        }

        private void SaveFile(string sourceFileName, string destinationFileName)
        {
            File.Copy(sourceFileName, destinationFileName);
        }

        private string GetNewFileName(FileInfo fileinfo)
        {
            var mp3File = new Mp3File { Name = fileinfo.Name, Size = fileinfo.Length };
            return mp3File.GetNewString(mp3File.Size, mp3File.Name);
        }
    }
}