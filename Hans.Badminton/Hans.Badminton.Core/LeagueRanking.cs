using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hans.Badminton.Core
{
    public class LeagueRanking
    {
        // player1 player2 point1 player3 player4 point2 different
        public string Winner1 { get; set; }
        public string Winner2 { get; set; }
        public string Loser1 { get; set; }
        public string Loser2 { get; set; }
        public int WinnerPoint { get; set; }
        public int LoserPoint { get; set; }
        public int Different { get; set; }

        public LeagueRanking(RawResult result)
        {
            SetWinner(result);
            SetPoint(result);
            SetDifferent();
        }

        private void SetWinner(RawResult result)
        {
            if (result.Point1 > result.Point2)
            {
                if (string.Compare(result.Player1, result.Player2) == -1)
                {
                    // player1 < player2
                    Winner1 = result.Player1;
                    Winner2 = result.Player2;
                }
                else
                {
                    // player2 < player1
                    Winner1 = result.Player2;
                    Winner2 = result.Player1;
                }

                if (string.Compare(result.Player3, result.Player4) == -1)
                {
                    // player3 < player4
                    Loser1 = result.Player3;
                    Loser2 = result.Player4;
                }
                else
                {
                    // player4 < player2
                    Loser1 = result.Player4;
                    Loser2 = result.Player3;
                }

                //Winner1 = result.Player1;
                //Winner2 = result.Player2;
                //Loser1 = result.Player3;
                //Loser2 = result.Player4;
            }
            else
            {
                if (string.Compare(result.Player3, result.Player4) == -1)
                {
                    // player3 < player4
                    Winner1 = result.Player3;
                    Winner2 = result.Player4;
                }
                else
                {
                    // player4 < player3
                    Winner1 = result.Player4;
                    Winner2 = result.Player3;
                }

                if (string.Compare(result.Player1, result.Player2) == -1)
                {
                    // player1 < player2
                    Loser1 = result.Player1;
                    Loser2 = result.Player2;
                }
                else
                {
                    // player4 < player3
                    Loser1 = result.Player2;
                    Loser2 = result.Player1;
                }

                //Winner1 = result.Player3;
                //Winner2 = result.Player4;
                //Loser1 = result.Player1;
                //Loser2 = result.Player2;
            }
        }

        private void SetPoint(RawResult result)
        {
            if (result.Point1 > result.Point2)
            {
                WinnerPoint = result.Point1;
                LoserPoint = result.Point2;
            }
            else
            {
                WinnerPoint = result.Point2;
                LoserPoint = result.Point1;
            }
        }

        private void SetDifferent()
        {
            Different = WinnerPoint - LoserPoint;
        }
    }
}
