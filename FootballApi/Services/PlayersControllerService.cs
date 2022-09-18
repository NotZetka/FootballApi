using FootballApi.Exceptions;
using FootballApi.Models;
using FootballApi.Models.Create;

namespace FootballApi.Services
{
    public interface IPlayersControllerService
    {
        Player add(CreatePlayer playerData);
        Player get(int id);
        IEnumerable<Player> getAll();
    }

    public class PlayersControllerService : IPlayersControllerService
    {
        private readonly FootballDBContext dbContext;

        public PlayersControllerService(FootballDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IEnumerable<Player> getAll()
        {
            return dbContext.Players.ToList();
        }
        public Player get(int id)
        {
            var player = dbContext.Players.FirstOrDefault(x => x.Id == id);
            if (player == null) throw new NotFoundException($"player with id:{id} not found");
            return player;
        }
        public Player add(CreatePlayer playerData)
        {
            Club club = dbContext.Clubs.Where(c => c.Name == "NoClub").FirstOrDefault();

            Player player = new()
            {
                Name = playerData.Name,
                Surname = playerData.Surname,
                Country = playerData.Country,
                Age = playerData.Age
            };
            club.Players.Add(player);
            dbContext.SaveChanges();
            return player;
        }
    }
}
