using FootballApi.Exceptions;
using FootballApi.Models;
using FootballApi.Models.Create;
using FootballApi.Models.Update;
using Microsoft.EntityFrameworkCore;

namespace FootballApi.Services
{
    public interface IClubsControllerService
    {
        Club addClub(CreateClub clubData);
        Club addPlayer(string name, CreatePlayer playerData);
        void delete(string name);
        Club get(string name);
        IEnumerable<Club> getAll();
        void movePlayer(string name, MovePlayer movePlayer);
        Club update(string name, UpdateClub clubData);
    }

    public class ClubsControllerService : IClubsControllerService
    {
        private readonly FootballDBContext dbContext;

        public ClubsControllerService(FootballDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<Club> getAll()
        {
            return dbContext.Clubs.Include(c => c.Players).ToList();
        }
        public Club get(string name)
        {
            var club = dbContext
                .Clubs
                .Include(c => c.Players)
                .Where(c => c.Name == name)
                .FirstOrDefault();
            if (club is null) throw new NotFoundException("Club not found");
            return club;
        }
        public void delete(string name)
        {
            var club = dbContext.Clubs.Where(c => c.Name == name).FirstOrDefault();
            if (club is null) throw new NotFoundException("Club not found");
            dbContext.Clubs.Remove(club);
            dbContext.SaveChanges();
        }
        public Club update(string name, UpdateClub clubData)
        {
            Club club = dbContext.Clubs.Where(c => c.Name == name).FirstOrDefault();
            if (club is null) throw new NotFoundException("Club not found");
            if (clubData.Country != "") { club.Country = clubData.Country; }
            if (clubData.City != "") { club.City = clubData.City; }
            dbContext.SaveChanges();
            return club;
        }
        public Club addClub(CreateClub clubData)
        {
            var clubNames = dbContext.Clubs.Select(c => c.Name).ToList();
            if (clubNames.Contains(clubData.Name)) throw new ConflictException("Name already exists");
            Club club = new()
            {
                Name = clubData.Name,
                City = clubData.City,
                Country = clubData.Country
            };
            dbContext.Clubs.Add(club);
            dbContext.SaveChanges();
            return club;
        }
        public Club addPlayer(string name, CreatePlayer playerData)
        {
            var club = dbContext.Clubs.Where(c => c.Name == name).FirstOrDefault();
            if (club is null) throw new NotFoundException("Club not found");

            Player player = new()
            {
                Name = playerData.Name,
                Surname = playerData.Surname,
                Country = playerData.Country,
                Age = playerData.Age
            };
            club.Players.Add(player);
            dbContext.SaveChanges();
            return club;
        }
        public void movePlayer(string name, MovePlayer movePlayer)
        {
            var club = dbContext.Clubs.Where(c => c.Name == name).FirstOrDefault();
            if (club is null) throw new NotFoundException("Club not found");
            var newClub = dbContext.Clubs.Where(c => c.Name == movePlayer.clubName).FirstOrDefault();
            if (newClub is null) throw new NotFoundException("New club not found");
            var player = dbContext.Players.FirstOrDefault(p => p.Id == movePlayer.playerId);
            if (player is null) throw new NotFoundException("Player not found");
            club.Players.Remove(player);
            newClub.Players.Add(player);
            dbContext.SaveChanges();
        }
    }
}
