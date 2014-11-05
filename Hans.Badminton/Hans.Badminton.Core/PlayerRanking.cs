using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hans.Badminton.Core
{
    public class PlayerRanking
    {
        public string Player { get; set; }
        public int Game { get; set; }
        public int Win { get; set; }
        public int WinPoint { get; set; }
        public int LosePoint { get; set; }
        public int Different { get; set; }
        public string Percentage { get; set; }
    }
}
