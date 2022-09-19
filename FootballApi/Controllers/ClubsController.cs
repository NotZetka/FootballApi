using FootballApi.Exceptions;
using FootballApi.Models;
using FootballApi.Models.Create;
using FootballApi.Models.Update;
using FootballApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FootballApi.Controllers
{
    [ApiController]
    [Route("FootballApi/Clubs")]
    [Authorize]
    public class ClubsController: ControllerBase
    {
        private readonly IClubsControllerService clubsControllerService;

        public ClubsController(IClubsControllerService clubsControllerService)
        {
            this.clubsControllerService = clubsControllerService;
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Club>> GetAll()
        {
            var clubs = clubsControllerService.getAll();
            return Ok(clubs);
        }
        [HttpGet("{name}")]
        public ActionResult<Club> Get([FromRoute]string name)
        {
            var club = clubsControllerService.get(name);
            return Ok(club);
        }
        [HttpDelete("{name}")]
        public ActionResult Delete([FromRoute]string name)
        {
            clubsControllerService.delete(name);
            return Ok();
        }
        [HttpPut("{name}")]
        public ActionResult<Club> Update([FromRoute] string name, [FromBody] UpdateClub clubData)
        {
            var club = clubsControllerService.update(name, clubData);
            return Ok(club);
        }
        [HttpPost("add")]
        public ActionResult AddClub([FromBody]CreateClub clubData)
        {
            if (!ModelState.IsValid) throw new BadRequestException("Invalid data");
            var club = clubsControllerService.addClub(clubData);
            return Created($"/{club.Name}", null);
        }
        [HttpPost("{name}/addPlayer")]
        public ActionResult AddPlayer([FromRoute] string name, [FromBody] CreatePlayer playerData)
        {
            if (!ModelState.IsValid) throw new Exception("Invalid data");
            var club = clubsControllerService.addPlayer(name, playerData);
            return Ok(club);
        }
        [HttpPut("{name}/movePlayer")]
        public ActionResult MovePlayer([FromRoute] string name, [FromBody] MovePlayer movePlayer)
        {
            if (!ModelState.IsValid) throw new Exception("Invalid data");
            clubsControllerService.movePlayer(name, movePlayer);
            return Ok();
        }
        
    }
}
