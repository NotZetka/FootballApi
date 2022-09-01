namespace FootballApi.Models
{
    public enum Role
    {
        User,
        Admin,
        HeadAdmin
    }
    public class ApiUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public Role Role { get; set; } = Role.User;
    }
}
