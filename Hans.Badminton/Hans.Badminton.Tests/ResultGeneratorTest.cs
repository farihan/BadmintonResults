using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hans.Badminton.Core;

namespace Hans.Badminton.Tests
{
    [TestClass]
    public class ResultGeneratorTest
    {
        [TestMethod]
        public void CanPopulateRawResults()
        {
            var file = @"C:\Downloads\Workspaces\Hans.Badminton\Hans.Badminton.Tests\bin\Debug\TestData_1.txt";

            var generator = new ResultGenerator(file);
            generator.PopulateRawResults();

            Assert.AreEqual(3, generator.RawResults.Count);
            Assert.AreEqual("POKD", generator.RawResults[0].Player1);
            Assert.AreEqual("ODEN", generator.RawResults[1].Player2);
            Assert.AreEqual(22, generator.RawResults[2].Point1);
            Assert.AreEqual("KAA", generator.RawResults[0].Player3);
            Assert.AreEqual("HANS", generator.RawResults[1].Player4);
            Assert.AreEqual(24, generator.RawResults[2].Point2);
        }

        [TestMethod]
        public void CanPopulateLeagueRankingResults()
        {
            var file = @"C:\Downloads\Workspaces\Hans.Badminton\Hans.Badminton.Tests\bin\Debug\TestData_1.txt";

            var generator = new ResultGenerator(file);
            generator.PopulateRawResults();
            generator.PopulateLeagueRankingResults();

            Assert.AreEqual(3, generator.RawResults.Count);
            Assert.AreEqual(3, generator.LeagueRankings.Count);

            Assert.AreEqual("POKD", generator.LeagueRankings[0].Winner1);
            Assert.AreEqual("KAA", generator.LeagueRankings[0].Loser1);
            Assert.AreEqual(21, generator.LeagueRankings[0].WinnerPoint);
            Assert.AreEqual(11, generator.LeagueRankings[0].LoserPoint);
            Assert.AreEqual(10, generator.LeagueRankings[0].Different);

            Assert.AreEqual("POKD", generator.LeagueRankings[2].Winner2);
            Assert.AreEqual("ANT", generator.LeagueRankings[2].Loser2);
            Assert.AreEqual(24, generator.LeagueRankings[2].WinnerPoint);
            Assert.AreEqual(22, generator.LeagueRankings[2].LoserPoint);
            Assert.AreEqual(2, generator.LeagueRankings[2].Different);
        }

        [TestMethod]
        public void CanPopulateRawPlayers()
        {
            var file = @"C:\Downloads\Workspaces\Hans.Badminton\Hans.Badminton.Tests\bin\Debug\TestData_1.txt";

            var generator = new ResultGenerator(file);
            generator.PopulateRawResults();
            generator.PopulateLeagueRankingResults();
            generator.PopulateRawPlayers();

            Assert.AreEqual(3, generator.RawResults.Count);
            Assert.AreEqual(3, generator.LeagueRankings.Count);
            Assert.AreEqual(12, generator.RawPlayers.Count);

            Assert.AreEqual("POKD", generator.RawPlayers[0].Initial);
            Assert.AreEqual(21, generator.RawPlayers[0].Point);
            Assert.AreEqual(true, generator.RawPlayers[0].IsWinner);

            Assert.AreEqual("KAA", generator.RawPlayers[2].Initial);
            Assert.AreEqual(11, generator.RawPlayers[2].Point);
            Assert.AreEqual(false, generator.RawPlayers[2].IsWinner);

            Assert.AreEqual("ODEN", generator.RawPlayers[8].Initial);
            Assert.AreEqual(24, generator.RawPlayers[8].Point);
            Assert.AreEqual(true, generator.RawPlayers[8].IsWinner);
        }

        [TestMethod]
        public void CanPopulatePlayerRankings()
        {
            var file = @"C:\Downloads\Workspaces\Hans.Badminton\Hans.Badminton.Tests\bin\Debug\TestData_2.txt";

            var generator = new ResultGenerator(file);
            generator.PopulateRawResults();
            generator.PopulateLeagueRankingResults();
            generator.PopulateRawPlayers();
            generator.PopulatePlayerRankings();

            Assert.AreEqual(7, generator.PlayerRankings.Count);
            //Assert.AreEqual("ODEN", generator.PlayerRankings[0].Player);

            foreach(var item in generator.PlayerRankings)
            {
                Console.WriteLine("{0} {1} {2} {3} {4} {5} {6}", item.Player, item.Game, item.Win, item.WinPoint, item.LosePoint, item.Different, item.Percentage);
            }
        }
    }
}
