using FootballApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FootballApi.Controllers
{
    [ApiController]
    [Route("FootballApi/Seeder")]
    public class Seeder : ControllerBase
    {
        private readonly FootballDBContext dBContext;

        public Seeder(FootballDBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        [HttpDelete("Clear")]
        public ActionResult Clear()
        {
            clear();
            return Ok();
        }
        [HttpPost("Recreate")]
        public ActionResult Recreate()
        {
            clear();
            Club FCBarcelona = new Club() { Name = "FCBarcelona", City = "Barcelona", Country = "Spain"};
            dBContext.Clubs.Add(FCBarcelona);
            Player Lewandowski = new Player() { Name = "Robert", Surname = "Lewandowksi", Age = 30, Country = "Poland" };
            Player Iniesta = new Player() { Name = "Andreas", Surname = "Iniesta", Age = 40, Country = "Spain" };
            Player Pique = new Player() { Name = "Gerard", Surname = "Pique", Age = 35, Country = "Spain" };
            FCBarcelona.Players.AddRange(new List<Player>{ Lewandowski, Iniesta, Pique});
            Club Psg = new Club() { Name = "Psg", City = "Paris", Country = "France"};
            dBContext.Clubs.Add(Psg);
            Player Messi = new Player() { Name = "Leo", Surname = "Messi", Age = 32, Country = "Argentina" };
            Player Mbappe = new Player() { Name = "Kylian", Surname = "Mbappe", Age = 23, Country = "France" };
            Player Neymar = new Player() { Name = "Neymar", Surname = "Neymar", Age = 30, Country = "Brasil" };
            Psg.Players.AddRange(new List<Player>{ Messi, Mbappe, Neymar });
            Club ManchesterUnited = new Club() { Name = "Manchester United", City = "Manchester", Country = "England" };
            dBContext.Clubs.Add(ManchesterUnited);
            Player Ronaldo = new Player() { Name = "Cristiano", Surname = "Ronaldo", Age = 37, Country = "Portugal" };
            Player deGea = new Player() { Name = "David", Surname = "de Gea", Age = 31, Country = "Spain" };
            ManchesterUnited.Players.AddRange(new List<Player>{ Ronaldo, deGea });
            dBContext.SaveChanges();
            return Ok();
        }

        void clear()
        {
            var players = dBContext.Players.ToList();
            dBContext.Players.RemoveRange(players);
            var clubs = dBContext.Clubs.ToList();
            dBContext.Clubs.RemoveRange(clubs);
            dBContext.SaveChanges();
        }
    }
}
