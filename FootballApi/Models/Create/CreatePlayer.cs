using System.ComponentModel.DataAnnotations;

namespace FootballApi.Models.Create
{
    public class CreatePlayer
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public string Country { get; set; }
    }
}
