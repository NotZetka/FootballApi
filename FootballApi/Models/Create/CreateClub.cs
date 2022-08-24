using System.ComponentModel.DataAnnotations;

namespace FootballApi.Models.Create
{
    public class CreateClub
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
    }
}
