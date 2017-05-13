using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisCourt.Models
{
    class Score : BindableBase
    {
        private string gameID;
        private int totalGames;
        private string serverName;
        private string receiverName;
        private string ballFlag;
        private string serverSet;
        private string receiverSet;
        private string serverScore;
        private string receiverScore;

        public string GameID
        {
            get { return gameID; }
            set { SetProperty(ref this.gameID, value); }
        }

        public int TotalGames
        {
            get { return totalGames; }
            set { SetProperty(ref this.totalGames, value); }
        }

        public string ServerName
        {
            get { return serverName; }
            set { SetProperty(ref this.serverName, value); }
        }

        public string ReceiverName
        {
            get { return receiverName; }
            set { SetProperty(ref this.receiverName, value); }
        }

        public string BallFlag
        {
            get { return ballFlag; }
            set { SetProperty(ref this.ballFlag, value); }
        }

        public string ServerSet
        {
            get { return serverSet; }
            set { SetProperty(ref this.serverSet, value); }
        }

        public string ReceiverSet
        {
            get { return receiverSet; }
            set { SetProperty(ref this.receiverSet, value); }
        }

        public string ServerScore
        {
            get { return serverScore; }
            set { SetProperty(ref this.serverScore, value); }
        }

        public string ReceiverScore
        {
            get { return receiverScore; }
            set { SetProperty(ref this.receiverScore, value); }
        }

        public Score(string _gameID, int _totalGames, string _serverName, string _receiverName, string _ballFlag, string _serverSet, string _receiverSet, string _serverScore, string _receiverScore)
        {
            gameID = _gameID;
            totalGames = _totalGames;
            serverName = _serverName;
            receiverName = _receiverName;
            ballFlag = _ballFlag;
            serverSet = _serverSet;
            receiverSet = _receiverSet;
            serverScore = _serverScore;
            receiverScore = _receiverScore;
        }
    }
}
