using FootballApi.Models;
using FootballApi.Models.Create;
using FootballApi.Models.Update;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FootballApi.Controllers
{
    [ApiController]
    [Route("FootballApi/Clubs")]
    public class ClubsController: ControllerBase
    {
        private readonly FootballDBContext dbContext;

        public ClubsController(FootballDBContext dBContext)
        {
            this.dbContext = dBContext;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Club>> GetAll()
        {
            var clubs = dbContext.Clubs.Include(c=>c.Players).ToList();
            return Ok(clubs);
        }
        [HttpGet("{name}")]
        public ActionResult<IEnumerable<Club>> Get([FromRoute]string name)
        {
            var club = dbContext
                .Clubs
                .Include(c => c.Players)
                .Where(c => c.Name == name)
                .FirstOrDefault();
            if (club is null) return NotFound();
            return Ok(club);
        }
        [HttpDelete("{name}")]
        public ActionResult Delete([FromRoute]string name)
        {
            var club = dbContext.Clubs.Where(c => c.Name == name).FirstOrDefault();
            if (club is null) return NotFound();
            dbContext.Clubs.Remove(club);
            dbContext.SaveChanges();
            return Ok();
        }
        [HttpPut("{name}")]
        public ActionResult<Club> Update([FromRoute] string name, [FromBody] UpdateClub clubData)
        {
            Club club = dbContext.Clubs.Where(c=>c.Name == name).FirstOrDefault();
            if (club is null) return NotFound();
            if (clubData.Country != "") { club.Country = clubData.Country; }
            if(clubData.City != "") { club.City = clubData.City; }
            dbContext.SaveChanges();
            return Ok(club);
        }
        [HttpPost("add")]
        public ActionResult AddClub([FromBody]CreateClub clubData)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            var clubNames = dbContext.Clubs.Select(c => c.Name).ToList();
            if (clubNames.Contains(clubData.Name)) return Conflict("Name already exists");
            Club club = new()
            {
                Name = clubData.Name,
                City = clubData.City,
                Country = clubData.Country                
            };
            dbContext.Clubs.Add(club);
            dbContext.SaveChanges();
            return Created($"/{club.Name}", null);
        }
        [HttpPost("{name}/addPlayer")]
        public ActionResult AddPlayer([FromRoute] string name, [FromBody] CreatePlayer playerData)
        {
            var club = dbContext.Clubs.Where(c => c.Name == name).FirstOrDefault();
            if(club is null) return NotFound(name);
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Player player = new()
            {
                Name = playerData.Name,
                Surname = playerData.Surname,
                Country = playerData.Country,
                Age = playerData.Age
            };
            club.Players.Add(player);
            dbContext.SaveChanges();
            return Ok(club);
        }
        [HttpPut("{name}/movePlayer")]
        public ActionResult MovePlayer([FromRoute] string name, [FromBody] MovePlayer movePlayer)
        {
            var club = dbContext.Clubs.Where(c => c.Name == name).FirstOrDefault();
            if (club is null) return NotFound(name);
            var newClub = dbContext.Clubs.Where(c => c.Name == movePlayer.clubName).FirstOrDefault();
            if (newClub is null) return NotFound("New club not found");
            var player = dbContext.Players.FirstOrDefault(p => p.Id == movePlayer.playerId);
            if (player is null) return NotFound("Player not found");
            club.Players.Remove(player);
            newClub.Players.Add(player);
            dbContext.SaveChanges();
            return Ok();
        }
        
    }
}
