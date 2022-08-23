namespace FootballApi.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string Country { get; set; }
        public virtual Club Club { get; set; }
    }
}
