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
        public ObservableCollection<Models.Games> allsets { get { return this.allsets; } }

        private Models.Games selectedSet = default(Models.Games);
        public Models.Games SelectSet
        {
            get { return selectedSet; }
            set { this.selectedSet = value; }
        }


        //add match
        public void AddMatch(string matchTitle, string matchID, DateTime start_date, DateTime end_date, List<string> categories, int total_set, int total_player, List<Games> game)
        {
            allMatches.Add(new Models.Matches(matchTitle, matchID, start_date, end_date, categories, total_set, total_player, game));
        }

        //remove match
        public void RemoveMatch()
        {
            this.allMatches.Remove(selectedMatch);
            this.selectedMatch = null;
        }

        //update match
        public void UpdateMatch(string matchTitle, DateTime start_date, DateTime end_date, List<string> categories, int total_set, int total_player, List<Games> game)
        {
            selectedMatch.MatchTitle = matchTitle;
            selectedMatch.Start_Date = start_date;
            selectedMatch.End_Date = end_date;
            selectedMatch.Categories = categories;
            selectedMatch.Total_Set = total_set;
            selectedMatch.Total_Player = total_player;
            selectedMatch.Game = game;
            this.selectedMatch = null;
        }


        //add game
        public void AddGame(string setID, string server, string receiver, string category, string umpire, string lineman, DateTime date, DateTime start_time, DateTime end_time, int court, string round, string result, List<string> gameScore, string status)
        {
            allSets.Add(new Models.Games(setID, server, receiver, category, umpire, lineman, date, start_time, end_time, court, round, result, gameScore, status));
        }

        //remove game
        public void RemoveGame()
        {
            this.allSets.Remove(selectedSet);
            this.selectedSet = null;
        }

        //update game
        public void UpdateGame(string server, string receiver, string category, string umpire, string lineman, DateTime date, DateTime start_time, DateTime end_time, int court, string round, string status)
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
    }
}
