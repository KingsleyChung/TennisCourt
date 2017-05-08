using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisCourt.Models
{
    class Matches : BindableBase
    {
        private string matchTitle;
        private string matchID;
        private DateTime start_date;
        private DateTime end_date;
        private List<string> categories;
        private int total_set;
        private int total_player;
        private List<Games> game;

        public string MatchTitle
        {
            get { return matchTitle; }
            set { SetProperty(ref this.matchTitle, value); }
        }
        private string MatchID
        {
            get { return matchID; }
            set { SetProperty(ref this.matchID, value); }
        }
        private DateTime Start_Date
        {
            get { return start_date; }
            set { SetProperty(ref this.start_date, value); }
        }
        private DateTime End_Date
        {
            get { return end_date; }
            set { SetProperty(ref this.end_date, value); }
        }
        private List<string> Categories
        {
            get { return categories; }
            set { SetProperty(ref this.categories, value); }
        }
        private int Total_Set
        {
            get { return total_set; }
            set { SetProperty(ref this.total_set, value); }
        }
        private int Total_Player
        {
            get { return total_set; }
            set { SetProperty(ref this.total_set, value); }
        }
        private List<Games> Game
        {
            get { return game; }
            set { SetProperty(ref this.game, value); }
        }

        public Matches(string _matchTitle, string _matchID, DateTime _start_date, DateTime _end_date, List<string> _categories, int _total_set, int _total_player, List<Games> _game)
        {
            matchTitle = _matchTitle;
            matchID = _matchID;
            start_date = _start_date;
            end_date = _end_date;
            categories = _categories;
            total_set = _total_set;
            total_player = _total_player;
            game = _game;
        }
    }
}
