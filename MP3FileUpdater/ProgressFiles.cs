using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;

namespace MP3FileUpdater.Core
{
    public class ProgressFiles : INotifyPropertyChanged
    {
        private int readedFiles;
        private int reamainingFiles;
        [Key]
        
        public int Id { get; set; }

        public int ReadedFilesCount
        {
            get
            { return readedFiles; }
            set
            {
                readedFiles = value;
                OnPropertyChanged(nameof(ReadedFilesCount));
            }
        }

       
        public int ReamainingFilesCount
        {
            get
            { return reamainingFiles; }
            set
            {
                reamainingFiles  = value;
                OnPropertyChanged(nameof(ReamainingFilesCount));
            }
        }
        
        public Mp3File mp3File { get; set; }
     
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
