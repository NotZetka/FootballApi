using Microsoft.AspNetCore.Mvc;

namespace FootballApi.Controllers
{
    [ApiController]
    [Route("")]
    public class ClubsController: ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Hello()
        {
            return Ok("Hello");
        }
    }
}
