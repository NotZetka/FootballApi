namespace FootballApi.Models
{
    public class Club
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public virtual List<Player> Players { get; set; } = new List<Player>();
    }
}
