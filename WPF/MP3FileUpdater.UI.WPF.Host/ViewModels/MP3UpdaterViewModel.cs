using Microsoft.WindowsAPICodePack.Dialogs;
using MP3FileUpdater.UI.WPF.Command;
using MP3Updater.Core;
using NLog;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MP3FileUpdater.UI.WPF.Host.ViewModels
{

    class MainViewModel : INotifyPropertyChanged
    {
        private string _sourcePath;
        private string _destinationPath;

        static Logger _logger = LogManager.GetCurrentClassLogger();

        private static object _lockObject = new object();

        private Mp3Directory _mp3Directory;
        private ProgressModel _progressModel;

        private ObservableCollection<ListBoxModel> _items = new ObservableCollection<ListBoxModel>();

        private bool _operationStarted;

        #region VMmodels        
        /// <summary>
        /// Gets or sets the progress model.
        /// </summary>
        /// <value>
        /// The progress model.
        /// </value>
        public ProgressModel ProgressModel
        {
            get
            {
                return _progressModel;
            }
            set
            {
                _progressModel = value;
                OnPropertyChanged("ProgressModel");
            }
        }

        public ObservableCollection<ListBoxModel> ListBoxModels
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;

                OnPropertyChanged("ListBoxModel");
            }
        }
        /// <summary>
        /// Gets or sets the source path.
        /// </summary>
        /// <value>
        /// The source path.
        /// </value>
        public string SourcePath
        {
            get
            {
                return _sourcePath;
            }
            set
            {
                _sourcePath = value;
                OnPropertyChanged();

                NotifyCommands();
            }
        }
        /// <summary>
        /// Gets or sets the destination path.
        /// </summary>
        /// <value>
        /// The destination path.
        /// </value>
        public string DestinationPath
        {
            get
            {
                return _destinationPath;
            }
            set
            {
                _destinationPath = value;
                OnPropertyChanged();

                NotifyCommands();
            }
        }
        #endregion


        /// <summary>
        /// Gets the Start operation command.
        /// </summary>
        /// <value>
        /// The start operation command.
        /// </value>

        public ICommand StartOperationCommand { get; }
        /// <summary>
        /// Gets the stop operation command.
        /// </summary>
        /// <value>
        /// The stop operation command.
        /// </value>
        public ICommand StopOperationCommand { get; }


        #region ..ctor        
        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            ProgressModel = new ProgressModel();
            StartOperationCommand = new AsyncCommand(StartOperationAsync, IsStartOperationEnabled);
            StopOperationCommand = new AsyncCommand(StopOperationAsync, IsStopOperationEnabled);
        }

        #endregion


        #region Commands handler        
        /// <summary>
        /// Determines whether [is stop operation enabled].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is stop operation enabled]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsStopOperationEnabled()
        {
            return _operationStarted;
        }

        /// <summary>
        /// Determines whether [is start operation enabled].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is start operation enabled]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsStartOperationEnabled()
        {
            return !_operationStarted && ValidatePathes();
        }

        /// <summary>
        /// Stops the files renaming
        /// </summary>
        private async Task StopOperationAsync()
        {
            await _mp3Directory?.StopProcess();

            lock (_lockObject)
            {
                _operationStarted = false;
            }

            NotifyCommands();
        }

        /// <summary>
        /// Starts the file updating process.
        /// </summary>
        private async Task StartOperationAsync()
        {
            _logger.Trace("Start button anabled");

            if (ValidatePathes())
            {
                _mp3Directory = new Mp3Directory(new System.IO.DirectoryInfo(SourcePath), new System.IO.DirectoryInfo(DestinationPath));
                _logger.Debug("check procces status before begin {@ _progressModel.IsIndeterminated}", _progressModel.IsIndeterminated);

                lock (_lockObject)
                {
                    _operationStarted = true;
                }
                NotifyCommands();

                var progress = new Progress<int>();

                progress.ProgressChanged += Progress_ProgressChanged;

                _mp3Directory.FilesSearchDone += (sender, args) =>
                {
                    _progressModel.IsIndeterminated = false;
                    _logger.Debug("check procces status after begin {@ _progressModel.IsIndeterminated}", _progressModel.IsIndeterminated);
                    _progressModel.FilesCount = args.FilesCount;
                    _logger.Debug("Fetchess files count {@_progressModel.FilesCount}", _progressModel.FilesCount);
                };
                foreach (var filename in _mp3Directory.GetFileName())
                {
                    var id = 0;
                    ListBoxModels.Add(new ListBoxModel() { Id = id, Name = filename });
                    id++;
                }

                await _mp3Directory.Process(5, progress);
            }
            _logger.Trace("Start button ended");
        }

        private void Progress_ProgressChanged(object sender, int e)
        {

            _progressModel.Value = e;
        }

        private bool ValidatePathes()
        {
            var result = false;
            if (SourcePath != null && DestinationPath != null)
            { result = true; }

            return result;

        }

        private void NotifyCommands()
        {
            ((AsyncCommand)StartOperationCommand).RaiseCanExecuteChanged();
            ((AsyncCommand)StopOperationCommand).RaiseCanExecuteChanged();
        }
        #endregion


        #region INotifyPropertyChanged impl
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Pathes Commands
        public ICommand CommandToGetSource
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    CommonOpenFileDialog dialog = new CommonOpenFileDialog();
                    dialog.InitialDirectory = SourcePath;
                    dialog.IsFolderPicker = true;

                    if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                    {
                        SourcePath = dialog.FileName;
                    }

                });

            }
        }

        public ICommand CommandToGetDestination
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    CommonOpenFileDialog dialog = new CommonOpenFileDialog();
                    dialog.InitialDirectory = DestinationPath;
                    dialog.IsFolderPicker = true;

                    if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                    {
                        DestinationPath = dialog.FileName;
                    }

                });

            }
        }
        #endregion

    }
}
