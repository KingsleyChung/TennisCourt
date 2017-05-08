using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisCourt.Models
{
    class Matches
    {
        private string matchTitle;
        private string matchID;
        private DateTime start_date;
        private DateTime end_date;
        private List<string> categories;
        private int total_set;
        private int total_player;
        private List<Games> games;
    }
}
