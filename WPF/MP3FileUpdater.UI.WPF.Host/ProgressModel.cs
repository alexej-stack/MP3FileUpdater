using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MP3FileUpdater.UI.WPF.Host
{
    public class ProgressModel:INotifyPropertyChanged
    {
        private int barvalue;
        private int filescount;
        private bool isindeterminate=true;
        public int Value {
            get
            { return barvalue; }
            set
            {
                barvalue = value;
                OnPropertyChanged("Value");

            }
        }
        public int FilesCount
        {
            get
            { return filescount; }
            set
            {
                filescount = value;
                OnPropertyChanged("FilesCount");

            }
        }
        public bool IsIndeterminated
        {
            get
            { return isindeterminate; }
            set
            {
                isindeterminate = value;
                OnPropertyChanged("IsIndeterminated");

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
