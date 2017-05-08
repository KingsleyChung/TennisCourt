using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TennisCourt.Models
{
    class Games : BindableBase
    {
        private string setID;
        private string server;
        private string receiver;
        private string category;
        private string umpire;
        private string lineman;
        private DateTime date;
        private DateTime start_time;
        private DateTime end_time;
        private int court;
        private string round;
        private string result;
        private List<string> gameScore;
        private string status;
    }
}

class BindableBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
    {
        if (object.Equals(storage, value))
            return false;
        storage = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}