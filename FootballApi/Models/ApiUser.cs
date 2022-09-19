namespace FootballApi.Models
{
    public class ApiUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
