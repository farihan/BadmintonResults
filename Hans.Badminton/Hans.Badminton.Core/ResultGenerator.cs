using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hans.Badminton.Core
{
    public class ResultGenerator
    {
        public string SourceFile { get; set; }
        public string TextFile { get; set; }

        public List<RawResult> RawResults { get; set; }
        public List<LeagueRanking> LeagueRankings { get; set; }
        public List<RawPlayer> RawPlayers { get; set; }
        public List<PlayerRanking> PlayerRankings { get; set; }
        public List<RawPair> RawPairs { get; set; }
        public List<PairRanking> PairRankings { get; set; }

        public ResultGenerator(string sourceFile, string textFile)
        {
            SourceFile = sourceFile;
            TextFile = textFile;

            RawResults = new List<RawResult>();
            LeagueRankings = new List<LeagueRanking>();
            RawPlayers = new List<RawPlayer>();
            PlayerRankings = new List<PlayerRanking>();
            RawPairs = new List<RawPair>();
            PairRankings = new List<PairRanking>();
        }

        public void PopulateRawResults()
        {
            var text = string.Empty;

            if (SourceFile != "")
                text = File.ReadAllText(SourceFile);
            else
                text = TextFile;

            var processText = text.ToLower().Replace("vs", "");
            var lines = processText.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                if (line.Contains("vs"))
                    line.Replace("vs", "");

                if (line.Contains("VS"))
                    line.Replace("VS", "");

                var words = new string[]{};

                if (line.Contains(" "))
                    words = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                if (line.Contains("\t"))
                    words = line.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);

                var regex = new Regex(@"^\d+$");

                if (regex.IsMatch(words[2]))
                {
                    RawResults.Add(new RawResult
                    {
                        Player1 = words[0].Trim().ToUpper(),
                        Player2 = words[1].Trim().ToUpper(),
                        Point1 = int.Parse(words[2]),
                        Player3 = words[3].Trim().ToUpper(),
                        Player4 = words[4].Trim().ToUpper(),
                        Point2 = int.Parse(words[5])
                    });
                }
                else
                {
                    RawResults.Add(new RawResult
                    {
                        Player1 = words[0].Trim().ToUpper(),
                        Player2 = words[1].Trim().ToUpper(),
                        Player3 = words[2].Trim().ToUpper(),
                        Player4 = words[3].Trim().ToUpper(),
                        Point1 = int.Parse(words[4]),
                        Point2 = int.Parse(words[5])
                    });
                }
            }
        }

        public void PopulateLeagueRankingResults()
        {
            foreach (var item in RawResults)
            {
                LeagueRankings.Add(new LeagueRanking(item));
            }
        }

        public void PopulateRawPlayers()
        {
            foreach (var item in LeagueRankings)
            {
                RawPlayers.Add(new RawPlayer
                {
                    Initial = item.Winner1,
                    Point = item.WinnerPoint,
                    IsWinner = true,
                    Different = item.WinnerPoint - item.LoserPoint
                });
                RawPlayers.Add(new RawPlayer
                {
                    Initial = item.Winner2,
                    Point = item.WinnerPoint,
                    IsWinner = true,
                    Different = item.WinnerPoint - item.LoserPoint
                });
                RawPlayers.Add(new RawPlayer
                {
                    Initial = item.Loser1,
                    Point = item.LoserPoint,
                    IsWinner = false,
                    Different = item.WinnerPoint - item.LoserPoint
                });
                RawPlayers.Add(new RawPlayer
                {
                    Initial = item.Loser2,
                    Point = item.LoserPoint,
                    IsWinner = false,
                    Different = item.WinnerPoint - item.LoserPoint
                });
            }
        }

        public void PopulatePlayerRankings()
        {
            var players = RawPlayers.Select(x => x.Initial).Distinct();

            foreach(var player in players)
            {
                var game = RawPlayers.Where(x => x.Initial == player).Count();
                var win = RawPlayers.Where(x => x.Initial == player && x.IsWinner == true).Count();
                var winPoint = RawPlayers.Where(x => x.Initial == player && x.IsWinner == true).Sum(x => x.Different);
                var losePoint = RawPlayers.Where(x => x.Initial == player && x.IsWinner == false).Sum(x => x.Different);
                var different = winPoint - losePoint;
                var percentage = ((decimal)win / game) * 100;

                PlayerRankings.Add(new PlayerRanking
                {
                    Player = player,
                    Game = game,
                    Win = win,
                    WinPoint = winPoint,
                    LosePoint = losePoint,
                    Different = different,
                    Percentage = Math.Round(percentage).ToString()
                });
            }

            // W>WD>DIFF>W%
            PlayerRankings = PlayerRankings
                .OrderByDescending(x => x.Win)
                .ThenByDescending(x => x.WinPoint)
                .ThenByDescending(x => x.Different)
                .ThenByDescending(x => x.Percentage)
                .ToList();
        }

        public void PopulateRawPair()
        {
            foreach (var item in LeagueRankings)
            {
                RawPairs.Add(new RawPair
                {
                    Initials = string.Format("{0} {1}", item.Winner1, item.Winner2),
                    Point = item.WinnerPoint,
                    IsWinner = true,
                    Different = item.WinnerPoint - item.LoserPoint
                });

                RawPairs.Add(new RawPair
                {
                    Initials = string.Format("{0} {1}", item.Loser1, item.Loser2),
                    Point = item.WinnerPoint,
                    IsWinner = false,
                    Different = item.WinnerPoint - item.LoserPoint
                });
            }
        }

        public void PopulatePairRankings()
        {
            var pairs = RawPairs.Select(x => x.Initials).Distinct();

            foreach (var pair in pairs)
            {
                var player = pair.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                var p1 = player[0];
                var p2 = player[1];
                var game = RawPairs.Where(x => x.Initials == pair).Count();
                var win = RawPairs.Where(x => x.Initials == pair && x.IsWinner == true).Count();
                var winPoint = RawPairs.Where(x => x.Initials == pair && x.IsWinner == true).Sum(x => x.Different);
                var losePoint = RawPairs.Where(x => x.Initials == pair && x.IsWinner == false).Sum(x => x.Different);
                var different = winPoint - losePoint;
                var percentage = ((decimal)win / game) * 100;

                PairRankings.Add(new PairRanking {
                    Player1 = p1,
                    Player2 = p2,
                    Game = game,
                    Win = win,
                    WinPoint = winPoint,
                    LosePoint = losePoint,
                    Different = different,
                    Percentage = Math.Round(percentage).ToString()
                });
            }

            // W>WD>DIFF>W%
            PairRankings = PairRankings
                .OrderByDescending(x => x.Win)
                .ThenByDescending(x => x.WinPoint)
                .ThenByDescending(x => x.Different)
                .ThenByDescending(x => x.Percentage)
                .ToList();
        }

        // 12, 9, 7
        // 7, 4, 2
        // 1
        public void PopulateOverallRankings()
        {
            var list = ReadFromSource();
            var pairs = list.Select(x => x.Initials).Distinct();

            foreach(var pair in pairs)
            {
                var player = pair.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                var p1 = player[0];
                var p2 = player[1];
                var game = list.Where(x => x.Initials == pair).Sum(x => x.Game);
                var win = list.Where(x => x.Initials == pair).Sum(x => x.Win);
                var winPoint = list.Where(x => x.Initials == pair).Sum(x => x.WinPoint);
                var losePoint = list.Where(x => x.Initials == pair).Sum(x => x.LosePoint);
                var different = winPoint - losePoint;
                var percentage = ((decimal)win / game) * 100;
                var point = list.Where(x => x.Initials == pair).Sum(x => x.Point);

                PairRankings.Add(new PairRanking
                {
                    Player1 = p1,
                    Player2 = p2,
                    Initials = pair,
                    Game = game,
                    Win = win,                   
                    WinPoint = winPoint,
                    LosePoint = losePoint,
                    Different = different,
                    Percentage = Math.Round(percentage).ToString(),
                    Point = point
                });
            }

            // PTS>W>WD>DIFF>W%
            PairRankings = PairRankings
                .OrderByDescending(x => x.Point)
                .ThenByDescending(x => x.Win)
                .ThenByDescending(x => x.WinPoint)
                .ThenByDescending(x => x.Different)
                .ThenByDescending(x => x.Percentage)
                .ToList();
        }

        private List<PairRanking> ReadFromSource()
        {
            var raws = new List<PairRanking>();

            var text = string.Empty;

            if (SourceFile != "")
                text = File.ReadAllText(SourceFile);
            else
                text = TextFile;

            var lines = text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var words = new string[] { };

                if (line.Contains(" "))
                    words = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                if (line.Contains("\t"))
                    words = line.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);

                var regex = new Regex(@"^\d+$");

                var p1 = words[0].Trim().ToUpper();
                var p2 = words[1].Trim().ToUpper();
                var initials = string.Format("{0} {1}", p1, p2);
                var game = int.Parse(words[2]);
                var win = int.Parse(words[3]);
                var lose = int.Parse(words[4]);
                var winPoint = int.Parse(words[5]);
                var losePoint = int.Parse(words[6]);
                var different = winPoint - losePoint;
                var percentage = ((decimal)win / game) * 100;
                var point = int.Parse(words[9]);

                raws.Add(new PairRanking
                {
                    Player1 = p1,
                    Player2 = p2,
                    Initials = initials,
                    Game = game,
                    Win = win,
                    WinPoint = winPoint,
                    LosePoint = losePoint,
                    Different = different,
                    Percentage = Math.Round(percentage).ToString(),
                    Point = point
                });
            }

            return raws;
        }
    }
}
