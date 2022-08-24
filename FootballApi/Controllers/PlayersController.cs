using FootballApi.Models;
using FootballApi.Models.Create;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FootballApi.Controllers
{
    [ApiController]
    [Route("FootballApi/Players")]
    public class PlayersController : ControllerBase
    {
        private readonly FootballDBContext dbContext;

        public PlayersController(FootballDBContext dBContext)
        {
            this.dbContext = dBContext;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Player>> GetAll()
        {
            var players = dbContext.Players.ToList();
            return Ok(players);
        }
        [HttpGet("{id}")]
        public ActionResult<Player> Get([FromRoute]int id)
        {
            var player = dbContext.Players.FirstOrDefault(x => x.Id == id);
            if(player == null) return NotFound("Player not found");
            return Ok(player);
        }
        [HttpPost("add")]
        public ActionResult Add([FromBody] CreatePlayer playerData)
        {
            Club club = dbContext.Clubs.Where(c => c.Name == "NoClub").FirstOrDefault();
            if(!ModelState.IsValid) return BadRequest(ModelState);
            Player player = new()
            {
                Name = playerData.Name,
                Surname = playerData.Surname,
                Country = playerData.Country,
                Age = playerData.Age
            };
            club.Players.Add(player);
            dbContext.SaveChanges();
            return Created($"/{player.Id}", null);
        }
    }
}
