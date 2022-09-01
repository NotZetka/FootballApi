using FootballApi.Models;
using FootballApi.Models.UserOperations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FootballApi.Controllers
{
    [ApiController]
    [Route("Users")]
    public class UserController : ControllerBase
    {
        private readonly FootballDBContext dBContext;
        private readonly IPasswordHasher<ApiUser> passwordHasher;

        public UserController(FootballDBContext dBContext, IPasswordHasher<ApiUser> passwordHasher)
        {
            this.dBContext = dBContext;
            this.passwordHasher = passwordHasher;
        }

        public IPasswordHasher<ApiUser> PasswordHasher { get; }

        [HttpPost("login")]
        public ActionResult<string> login([FromBody]LoginUser data)
        {
            var user = dBContext.Users.Where(u=>u.Email==data.Email).FirstOrDefault();
            if(user is null)
            {
                return BadRequest("Invalid email");
            }
            var result = passwordHasher.VerifyHashedPassword(user,user.HashedPassword,data.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return BadRequest("Invalid password");
            }
            return Ok("");
        }
        [HttpPost("register")]
        public ActionResult register([FromBody]CreateUser data)
        {
            var mails = dBContext.Users.Select(u => u.Email).ToList();
            if (mails.Contains(data.Email))
            {
                return BadRequest("Email is already taken, try to login");
            }
            
            ApiUser user = new() { Email = data.Email };
            var hashedPassword = passwordHasher.HashPassword(user, data.Password);
            user.HashedPassword = hashedPassword;
            dBContext.Users.Add(user);
            dBContext.SaveChanges();
            return Ok();
        }
    }
}
