using FootballApi.Exceptions;
using FootballApi.Models;
using FootballApi.Models.UserOperations;
using FootballApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FootballApi.Controllers
{

    [Route("Users")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserControllerService userControllerService;

        public UserController(IUserControllerService userControllerService)
        {
            this.userControllerService = userControllerService;
        }


        [HttpPost("login")]
        public ActionResult<string> login([FromBody]LoginUser data)
        {
            return Ok(userControllerService.login(data));
        }
        [HttpPost("register")]
        public ActionResult register([FromBody]CreateUser data)
        {
            userControllerService.register(data);
            return Ok();
        }
        [HttpGet]
        public ActionResult test()
        {
            return Ok();
        }
    }
}
