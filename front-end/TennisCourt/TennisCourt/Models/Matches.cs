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
        private int total_player;
        private List<Games> game;

        public string MatchTitle
        {
            get { return matchTitle; }
            set { SetProperty(ref this.matchTitle, value); }
        }
        public string MatchID
        {
            get { return matchID; }
            set { SetProperty(ref this.matchID, value); }
        }
        public DateTime Start_Date
        {
            get { return start_date; }
            set { SetProperty(ref this.start_date, value); }
        }
        public DateTime End_Date
        {
            get { return end_date; }
            set { SetProperty(ref this.end_date, value); }
        }
        public List<string> Categories
        {
            get { return categories; }
            set { SetProperty(ref this.categories, value); }
        }
        public int Total_Player
        {
            get { return total_player; }
            set { SetProperty(ref this.total_player, value); }
        }
        public List<Games> Game
        {
            get { return game; }
            set { SetProperty(ref this.game, value); }
        }

        public Matches(string _matchTitle, string _matchID, DateTime _start_date, DateTime _end_date, List<string> _categories, int _total_player, List<Games> _game)
        {
            matchTitle = _matchTitle;
            matchID = _matchID;
            start_date = _start_date;
            end_date = _end_date;
            categories = _categories;
            total_player = _total_player;
            game = _game;
        }
    }
}
