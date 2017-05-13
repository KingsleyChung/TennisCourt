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
        private ObservableCollection<Models.Matches> allMatches = new ObservableCollection<Models.Matches>();
        public ObservableCollection<Models.Matches> AllMatches { get { return this.allMatches; } }

        private Models.Matches selectedMatch = default(Models.Matches);
        public Models.Matches SelectMatch
        {
            get { return selectedMatch; }
            set { this.selectedMatch = value; }
        }
        // Games data
        private ObservableCollection<Models.Games> allSets = new ObservableCollection<Models.Games>();
        public ObservableCollection<Models.Games> AllSets { get { return this.allSets; } }

        private Models.Games selectedSet = default(Models.Games);
        public Models.Games SelectSet
        {
            get { return selectedSet; }
            set { this.selectedSet = value; }
        }
        // SpecialGames data
        private ObservableCollection<Models.Games> allspecialSets = new ObservableCollection<Models.Games>();
        public ObservableCollection<Models.Games> AllSpecialSets { get { return this.allspecialSets; } }

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

        //add match
        public void AddMatch(string matchTitle, string matchID, DateTime start_date, DateTime end_date, string categories, string total_player, string status, List<Games> game)
        {
            allMatches.Add(new Models.Matches(matchTitle, matchID, start_date, end_date, categories, total_player, status, game));
        }
        /*
        //remove match
        public void RemoveMatch()
        {
            this.allMatches.Remove(selectedMatch);
            this.selectedMatch = null;
        }

        //update match
        public void UpdateMatch(string matchTitle, DateTime start_date, DateTime end_date, List<string> categories, int total_set, int total_player)
        {
            selectedMatch.MatchTitle = matchTitle;
            selectedMatch.Start_Date = start_date;
            selectedMatch.End_Date = end_date;
            selectedMatch.Categories = categories;
            selectedMatch.Total_Player = total_player;
            this.selectedMatch = null;
        }
        */
        //add game
        public void AddGame(string setID, string server, string receiver, string category, string umpire, string lineman, DateTime date, DateTime start_time, DateTime end_time, string court, string round, string result, List<string> gameScore, string status)
        {
            allSets.Add(new Models.Games(setID, server, receiver, category, umpire, lineman, date, start_time, end_time, court, round, result, gameScore, status));
        }
        /*
        //remove game
        public void RemoveGame()
        {
            this.allSets.Remove(selectedSet);
            this.selectedSet = null;
        }

        //update game
        public void UpdateGame(string server, string receiver, string category, string umpire, string lineman, DateTime date, DateTime start_time, DateTime end_time, string court, string round, string status)
        {
            selectedSet.Server = server;
            selectedSet.Receiver = receiver;
            selectedSet.Category = category;
            selectedSet.Umpire = umpire;
            selectedSet.Lineman = lineman;
            selectedSet.Date = date;
            selectedSet.Start_Time = start_time;
            selectedSet.End_Time = end_time;
            selectedSet.Court = court;
            selectedSet.Round = round;
            selectedSet.Status = status;

            this.selectedSet = null;
        }
        */
        /*
        //a example for test of HomePage
        public MatchesViewModel()
        {
            var currentTime = System.DateTime.Now;
            List<string> t = new List<string>();
            this.allSets.Add(new Models.Games("", "", "", "", "", "", currentTime, currentTime, currentTime, "court 1", "round 2", "", t, "-1"));
        }
        */

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
