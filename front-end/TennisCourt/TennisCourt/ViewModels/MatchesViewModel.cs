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
        public ObservableCollection<Models.Games> AllSets { get { return this.allSets; } }

        private Models.Matches selectedSet = default(Models.Matches);
        public Models.Matches SelectSet
        {
            get { return selectedSet; }
            set { this.selectedSet = value; }
        }

        //add match
        public void AddMatch(string matchTitle, int matchID, DateTime start_date, DateTime end_date, List<string> categories, int total_set, int total_player, List<Games> game)
        {
            allMatches.Add(new Models.Matches(matchTitle, matchID, start_date, end_date, categories, total_set, total_player, game));
        }

        //remove match
        public void RemoveMatch(int matchID)
        {
            this.allMatches.RemoveAt(matchID);
            this.selectedMatch = null;
        }

        //update match
        public void UpdateMatch(int _matchID, int _total_set, List<Games> _game)
        {
            for (int i = 0; i < this.allMatches.Count; i++)
            {
                Models.Matches match = this.allMatches.ElementAt(i);
                /*if (allMatches[i].matchID == _matchID)
                {
                    allMatches[i].matchID = _matchID;
                    allMatches[i].total_set = _total_set;
                    //allMatches[i].game = _game;
                    allMatches.SetItem(i, match);
                }*/
            }
            this.selectedMatch = null;
        }

        //add game
        public void AddGame(int setID, string server, string receiver, string category, string umpire, string lineman, DateTime date, DateTime start_time, DateTime end_time, int court, string round, string result, List<string> gameScore, string status)
        {
            allSets.Add(new Models.Games(setID, server, receiver, category, umpire, lineman, date, start_time, end_time, court, round, result, gameScore, status));
        }

        //remove game
        public void RemoveGame(int setID)
        {
            this.allSets.RemoveAt(setID);
            this.selectedSet = null;
        }

        //update game
        public void UpdateGame(int _setID, string _result, List<string> _gameScore)
        {
            for (int i = 0; i < this.allSets.Count; i++)
            {
                Models.Games game = this.allSets.ElementAt(i);
                /*if (allSets[i].setID == _setID)
                {
                    allSets[i].setID = _setID;
                    allSets[i].result = _result;
                    //allSets[i].gameScore = _gameScore;
                    allSets.SetItem(i, game);
                }*/
            }
            this.selectedSet = null;
        }
    }
}
