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

        public void AddMatch(string matchTitle, string matchID, DateTime start_date, DateTime end_date, List<string> categories, int total_set, int total_player, List<Games> game)
        {
            allMatches.Add(new Models.Matches(matchTitle, matchID, start_date, end_date, categories, total_set, total_player, game));
        }
    }
}
