using FootballApi.Models;
using FootballApi.Models.Create;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FootballApi.Controllers
{
    [ApiController]
    [Route("FootballApi")]
    public class ClubsController: ControllerBase
    {
        private readonly FootballDBContext dBContext;

        public ClubsController(FootballDBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Club>> GetAll()
        {
            var clubs = dBContext.Clubs.ToList();
            return Ok(clubs);
        }
        [HttpGet("{name}")]
        public ActionResult<IEnumerable<Club>> Get([FromRoute]string name)
        {
            var club = dBContext
                .Clubs
                .Include(c => c.Players)
                .Where(c => c.Name == name)
                .FirstOrDefault();
            return Ok(club);
        }
        [HttpPost("add")]
        public ActionResult AddClub([FromBody]CreateClub clubData)
        {
            Club club = new()
            {
                Name = clubData.Name,
                City = clubData.City,
                Country = clubData.Country                
            };
            dBContext.Clubs.Add(club);
            dBContext.SaveChanges();
            return Created($"/{club.Name}", null);
        }
        [HttpPost("{name}/add")]
        public ActionResult AddPlayer([FromRoute] string name, [FromBody]CreatePlayer playerData)
        {
            Player player = new()
            {
                Name = playerData.Name,
                Surname = playerData.Surname,
                Country = playerData.Country,
                Age = playerData.Age
            };
            Club club = dBContext.Clubs.Where(c => c.Name == name).FirstOrDefault();
            club.Players.Add(player);
            dBContext.SaveChanges();
            return Created($"/{club.Name}", null);
        }
    }
}
