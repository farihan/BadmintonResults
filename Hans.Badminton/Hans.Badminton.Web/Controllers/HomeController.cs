using Hans.Badminton.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hans.Badminton.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            try
            {
                string result = new StreamReader(file.InputStream).ReadToEnd();

                var generator = new ResultGenerator(string.Empty, result);
                generator.PopulateRawResults();
                generator.PopulateLeagueRankingResults();
                generator.PopulateRawPlayers();
                generator.PopulatePlayerRankings();
                generator.PopulateRawPair();
                generator.PopulatePairRankings();

                return View(generator);
            }
            catch(Exception ex)
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult Error()
        {
            return View();
        }

        public FilePathResult GetFileFromDisk()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Log\\";
            string fileName = "sample.txt";
            return File(path + fileName, "text/plain", "sample.txt");
        }

        public ActionResult NormalLeague()
        {
            ViewBag.Message = "Normal League.";

            var s = string.Format("~/Log/normal_{0}.txt", "2014-10-21");
            //var file = Server.MapPath("~/Log/normal_league.txt");
            var file = Server.MapPath(s);
            var generator = new ResultGenerator(file, string.Empty);
            generator.PopulateRawResults();
            generator.PopulateLeagueRankingResults();
            generator.PopulateRawPlayers();
            generator.PopulatePlayerRankings();
            generator.PopulateRawPair();
            generator.PopulatePairRankings();

            return View(generator);
        }
        
        public ActionResult PremierLeague()
        {
            ViewBag.Message = "Premier League.";

            var file = Server.MapPath("~/Log/premier_league.txt");
            var generator = new ResultGenerator(file, string.Empty);
            generator.PopulateRawResults();
            generator.PopulateLeagueRankingResults();
            generator.PopulateRawPlayers();
            generator.PopulatePlayerRankings();
            generator.PopulateRawPair();
            generator.PopulatePairRankings();

            return View(generator);
        }

        public ActionResult SuperLeague()
        {
            ViewBag.Message = "Super League.";

            var file = Server.MapPath("~/Log/super_league.txt");
            var generator = new ResultGenerator(file, string.Empty);
            generator.PopulateRawResults();
            generator.PopulateLeagueRankingResults();
            generator.PopulateRawPlayers();
            generator.PopulatePlayerRankings();
            generator.PopulateRawPair();
            generator.PopulatePairRankings();

            return View(generator);
        }

        public ActionResult Overall()
        {
            ViewBag.Message = "Overall.";

            var file = Server.MapPath("~/Log/overall.txt");
            var generator = new ResultGenerator(file, string.Empty);
            generator.PopulateOverallRankings();

            return View(generator);
        }
    }
}