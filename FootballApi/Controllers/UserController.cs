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
    [ApiController]
    [Route("Users")]
    public class UserController : ControllerBase
    {

        private readonly UserControllerService userControllerService;

        public UserController(UserControllerService userControllerService)
        {
            this.userControllerService = userControllerService;
        }

        public IPasswordHasher<ApiUser> PasswordHasher { get; }

        [HttpPost("login")]
        public ActionResult<string> login([FromBody]LoginUser data)
        {
            return userControllerService.login(data);
        }
        [HttpPost("register")]
        public ActionResult register([FromBody]CreateUser data)
        {
            userControllerService.register(data);
            return Ok();
        }
    }
}
