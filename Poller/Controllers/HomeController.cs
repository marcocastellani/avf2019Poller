using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Poller.Models;

namespace Poller.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        public IActionResult Votazione(string key)
        {
            return View("Poll", key);
        }

        public IActionResult Up(string key, string value)
        {
            var repo = new SqLiteBaseRepository();
            repo.Save(key,value);
        
            return View("Poll", key);

        }
        
        
        
        
        
        
        public class SqLiteBaseRepository
        {
            public static string DbFile
            {
                get { return Environment.CurrentDirectory + "/SimpleDb.sqlite"; }
            }

            public static SQLiteConnection SimpleDbConnection()
            {
                return new SQLiteConnection("Data Source=" + DbFile);
            }
            
            public void Save(string session, string vote)
            {
                if (!System.IO.File.Exists(DbFile))
                {
                    CreateDatabase();
                }

                using (var cnn = SimpleDbConnection())
                {
                    cnn.Open();
                     cnn.Execute(
                        @"INSERT INTO Votes 
            ( Session, Vote , EventDate) VALUES 
            ( @Session, @vote ,@EventDate ); ", new {Session=session, Vote=vote, EventDate=DateTime.Now});
                }
            }

            private static void CreateDatabase()
            {
                using (var cnn = SimpleDbConnection())
                {
                    cnn.Open();
                    cnn.Execute(
                        @"create table Votes
              (
                 ID                                  INTEGER PRIMARY KEY AUTOINCREMENT,
                 Session                           varchar(100) not null,
                 Vote                            varchar(100) not null,
                 EventDate                         datetime not null 
              )");
                }
            }
        }
    }
}