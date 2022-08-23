using FootballApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FootballApi.Controllers
{
    [ApiController]
    [Route("")]
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
    }
}
