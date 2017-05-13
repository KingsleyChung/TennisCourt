using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TennisCourt.Models;

namespace TennisCourt.ViewModels
{
    class MatchesViewModel
    {
        // User data
        private Models.User currentUser;
        public Models.User CurrentUser { get { return this.currentUser; } }

        // Matches data
        private MyObservableCollection<Models.Matches> allMatches = new MyObservableCollection<Models.Matches>();
        public MyObservableCollection<Models.Matches> AllMatches { get { return this.allMatches; } }

        private Models.Matches selectedMatch = default(Models.Matches);
        public Models.Matches SelectMatch
        {
            get { return selectedMatch; }
            set { this.selectedMatch = value; }
        }
        // Games data
        private MyObservableCollection<Models.Games> allSets = new MyObservableCollection<Models.Games>();
        public MyObservableCollection<Models.Games> AllSets { get { return this.allSets; } }

        private Models.Games selectedSet = default(Models.Games);
        public Models.Games SelectSet
        {
            get { return selectedSet; }
            set { this.selectedSet = value; }
        }
        // SpecialGames data
        private MyObservableCollection<Models.Games> allspecialSets = new MyObservableCollection<Models.Games>();
        public MyObservableCollection<Models.Games> AllSpecialSets { get { return this.allspecialSets; } }

        private Models.Games selectedspecialSet = default(Models.Games);
        public Models.Games SelectSpecialSet
        {
            get { return selectedspecialSet; }
            set { this.selectedspecialSet = value; }
        }
        //GameScore data
        private MyObservableCollection<Models.Score> allgameScore = new MyObservableCollection<Models.Score>();
        public MyObservableCollection<Models.Score> AllGameScore { get { return this.allgameScore; } }

        private Models.Score selectedgameScore = default(Models.Score);
        public Models.Score SelectGameScore
        {
            get { return selectedgameScore; }
            set { this.selectedgameScore = value; }
        }

        // add User
        public void AddUser(string userName, string studentID, string password, string mode)
        {
            currentUser = new Models.User(userName, studentID, password, mode);
        }

        //add GameScore
        public void AddGameScore(string gameID, int totalGames, string serverName, string receiverName, string ballFlag, string serverSet, string receiverSet, string serverScore, string receiverScore, string buttonFlag)
        {
            allgameScore.Add(new Models.Score(gameID, totalGames, serverName, receiverName, ballFlag, serverSet, receiverSet, serverScore, receiverScore, buttonFlag));
        }
        //update GameScore
        public void UpdateGameScore(string gameID, string serverScore, string receiverScore)
        {
            for (int i = 0; i < allgameScore.Count; i++)
            {
                Models.Score select = this.allgameScore.ElementAt(i);
                if (select.GameID == gameID)
                {
                    allgameScore[i].ServerScore = serverScore;
                    allgameScore[i].ReceiverScore = receiverScore;
                    allgameScore.SetItem(i, select);
                }
            }
        }
        //change button flag
        public void ChangeButtonFlag(string gameID, string flag)
        {
            for (int i = 0; i < allgameScore.Count; i++)
            {
                Models.Score select = this.allgameScore.ElementAt(i);
                if (select.GameID == gameID)
                {
                    allgameScore[i].ButtonFlag = flag;
                    allgameScore.SetItem(i, select);
                }
            }
        }

        //add specialgame
        public void AddSpecialGame(string setID, string server, string receiver, string category, string umpire, string lineman, DateTime date, DateTime start_time, DateTime end_time, string court, string round, string result, List<string> gameScore, string status)
        {
            allspecialSets.Add(new Models.Games(setID, server, receiver, category, umpire, lineman, date, start_time, end_time, court, round, result, gameScore, status));
        }
        //update game status
        public void UpdateSpecialGameStatus(string setID, string status)
        {
            for (int i = 0; i < allspecialSets.Count; i++)
            {
                Models.Games select = this.allspecialSets.ElementAt(i);
                if (select.SetID == setID)
                {
                    allspecialSets[i].Status = status;
                    allspecialSets.SetItem(i, select);
                }
            }
        }
        //update game end time
        public void UpdateSpecialGameTime(string setID, DateTime endTime)
        {
            for (int i = 0; i < allspecialSets.Count; i++)
            {
                Models.Games select = this.allspecialSets.ElementAt(i);
                if (select.SetID == setID)
                {
                    allspecialSets[i].End_Time = endTime;
                    allspecialSets.SetItem(i, select);
                }
            }
        }

        //add match
        public void AddMatch(string matchTitle, string matchID, DateTime start_date, DateTime end_date, string categories, string total_player, string status, List<Games> game)
        {
            allMatches.Add(new Models.Matches(matchTitle, matchID, start_date, end_date, categories, total_player, status, game));
        }

        //update match status
        public void UpdateMatchStatus(string matchID, string status)
        {
            for (int i = 0; i < allMatches.Count; i++)
            {
                Models.Matches select = this.allMatches.ElementAt(i);
                if (select.MatchID == matchID)
                {
                    allMatches[i].Status = status;
                    allMatches.SetItem(i, select);
                }
            }
        }
        //update match end time
        public void UpdateMatchTime(string matchID, DateTime endDate)
        {
            for (int i = 0; i < allMatches.Count; i++)
            {
                Models.Matches select = this.allMatches.ElementAt(i);
                if (select.MatchID == matchID)
                {
                    allMatches[i].End_Date = endDate;
                    allMatches.SetItem(i, select);
                }
            }
        }

        //add game
        public void AddGame(string setID, string server, string receiver, string category, string umpire, string lineman, DateTime date, DateTime start_time, DateTime end_time, string court, string round, string result, List<string> gameScore, string status)
        {
            allSets.Add(new Models.Games(setID, server, receiver, category, umpire, lineman, date, start_time, end_time, court, round, result, gameScore, status));
        }

        // get court status
        public ObservableCollection<Models.Games> CourtStatus = new ObservableCollection<Models.Games>();
        public void initializeCourtStatus()
        {
            Models.Games[] courtStatus = new Models.Games[6];
            foreach (Models.Matches match in allMatches)
            {
                foreach(Models.Games game in match.Game)
                {
                    if (game.Status != "1")
                    {
                        courtStatus[int.Parse(game.Court) - 1] = game;
                    }
                }
            }
            for (int i = 0; i < 6; i++)
            {
                if (courtStatus[i] != null)
                {
                    CourtStatus.Add(courtStatus[i]);
                }
                else
                {
                    CourtStatus.Add(new Games("", "", "", "", "", "", DateTime.Now, DateTime.Now, DateTime.Now, (i+1).ToString(), "", "", null, "-1"));
                }
            }
        }
    }
}
