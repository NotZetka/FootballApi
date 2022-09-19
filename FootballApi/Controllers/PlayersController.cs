using FootballApi.Exceptions;
using FootballApi.Models;
using FootballApi.Models.Create;
using FootballApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FootballApi.Controllers
{
    [ApiController]
    [Route("FootballApi/Players")]
    [Authorize]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayersControllerService playersControllerService;

        public PlayersController(IPlayersControllerService playersControllerService)
        {
            this.playersControllerService = playersControllerService;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Player>> GetAll()
        {
            var players = playersControllerService.getAll();
            return Ok(players);
        }
        [HttpGet("{id}")]
        public ActionResult<Player> Get([FromRoute]int id)
        {
            var player = playersControllerService.get(id);
            return Ok(player);
        }
        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public ActionResult Add([FromBody] CreatePlayer playerData)
        {
            if (!ModelState.IsValid) throw new BadRequestException("Invalid data");
            var player = playersControllerService.add(playerData);
            return Created($"/{player.Id}", null);
        }
    }
}
