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

        public string SetID
        {
            get { return setID; }
            set { SetProperty(ref this.setID, value); }
        }
        public string Server
        {
            get { return server; }
            set { SetProperty(ref this.server, value); }
        }
        public string Receiver
        {
            get { return receiver; }
            set { SetProperty(ref this.receiver, value); }
        }
        public string Category
        {
            get { return category; }
            set { SetProperty(ref this.category, value); }
        }
        public string Umpire
        {
            get { return umpire; }
            set { SetProperty(ref this.umpire, value); }
        }
        public string Lineman
        {
            get { return lineman; }
            set { SetProperty(ref this.lineman, value); }
        }
        public DateTime Date
        {
            get { return date; }
            set { SetProperty(ref this.date, value); }
        }
        public DateTime Start_Time
        {
            get { return start_time; }
            set { SetProperty(ref this.start_time, value); }
        }
        public DateTime End_Time
        {
            get { return end_time; }
            set { SetProperty(ref this.end_time, value); }
        }
        public int Court
        {
            get { return court; }
            set { SetProperty(ref this.court, value); }
        }
        public string Round
        {
            get { return round; }
            set { SetProperty(ref this.round, value); }
        }
        public string Result
        {
            get { return result; }
            set { SetProperty(ref this.result, value); }
        }
        public List<string> GameScore
        {
            get { return gameScore; }
            set { SetProperty(ref this.gameScore, value); }
        }
        public string Status
        {
            get { return status; }
            set { SetProperty(ref this.status, value); }
        }

        public Games(string _setID, string _server, string _receiver, string _category, string _umpire, string _lineman, DateTime _date, DateTime _start_time, DateTime _end_time, int _court, string _round, string _result, List<string> _gameScore, string _status)
        {
            setID = _setID;
            server = _server;
            receiver = _receiver;
            category = _category;
            umpire = _umpire;
            lineman = _lineman;
            date = _date;
            start_time = _start_time;
            end_time = _end_time;
            court = _court;
            round = _round;
            result = _result;
            gameScore = _gameScore;
            status = _status;
        }

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