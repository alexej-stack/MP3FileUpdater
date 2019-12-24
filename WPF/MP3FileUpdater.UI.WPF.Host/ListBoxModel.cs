using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MP3FileUpdater.UI.WPF.Host
{
   public  class ListBoxModel : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
    

        public int Id
        {
            get
            { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");

            }
        }
        public string Name
        {
            get
            { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");

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
