using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hans.Badminton.Core
{
    public class PairRanking
    {
        public string Player1 { get; set; }
        public string Player2 { get; set; }
        public string Initials { get; set; }
        public int Game { get; set; }
        public int Win { get; set; }
        public int WinPoint { get; set; }
        public int LosePoint { get; set; }
        public int Different { get; set; }
        public string Percentage { get; set; }         
        public int Point { get; set; }
    }
}
