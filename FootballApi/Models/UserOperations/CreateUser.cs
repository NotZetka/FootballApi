using System.ComponentModel.DataAnnotations;

namespace FootballApi.Models.UserOperations
{
    public class CreateUser
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
