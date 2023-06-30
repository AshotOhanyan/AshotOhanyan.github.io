using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestData.Data;
using TestData.DbModels;

namespace TestData.Repositories.GameRepository
{
    public class GameRepository : IGameRepository
    {
        private readonly DBContext dbContext;

        public GameRepository(DBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Game> AddDbObjectAsync(Game entity)
        {
            using (DBContext context = new DBContext())
            {
                Game game = await context.Games.FirstOrDefaultAsync(x => x.Id == entity.Id);

                if (game != null)
                    throw new Exception("Game already exists!");

                User currUser = new User();

                try
                {
                    game = new Game
                    {
                        Id = Guid.NewGuid(),
                        Title = string.IsNullOrEmpty(entity.Title) ? throw new Exception("Game most has a title!") : entity.Title,
                        Price = entity.Price ?? throw new Exception("Game most has a price!"),
                        Description = entity.Description,
                        Rate = entity.Rate,
                        ImageUrl = entity.ImageUrl,
                        UserId = entity.UserId,
                    };

                    if (entity.UserId != null && entity.UserId != Guid.Empty)
                    {
                        currUser = await context.Users.FirstOrDefaultAsync(x => x.Id == entity.UserId) ?? throw new Exception("User does not exist!");
                        if (currUser != null)
                        {
                            if (currUser.Games == null)
                            {
                                currUser.Games = new List<Game>();
                            }

                            currUser.Games.Add(game);
                            game.UserId = currUser.Id;
                        }
                    }

                    await context.AddAsync(game);
                    await context.SaveChangesAsync();
                }
                catch
                {
                    throw new Exception("Error while adding game to database!");
                }

                return game;
            }
        }

        public async Task DeleteDbObjectAsync(Guid id)
        {
            using (DBContext context = new DBContext())
            {
                Game game = await context.Games.FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Game already exists!");

                try
                {
                    context.Remove(game);
                    await context.SaveChangesAsync();
                }
                catch
                {
                    throw new Exception("Error while removing game from database!");
                }
            }
        }

        public async Task<IEnumerable<Game>> GetAllDbObjectsAsync()
        {
            using (DBContext context = new DBContext())
            {
                return await context.Games.Include(g => g.User).ToListAsync();
            }
        }

        public IQueryable<Game> GetAllDbObjectsByFilterAsync(Game entity)
        {
            IQueryable<Game> filteredGames;

            filteredGames = dbContext.Games.Include(g => g.User).AsQueryable();
            try
            {
                if (!string.IsNullOrEmpty(entity.Title))
                {
                    filteredGames = filteredGames.Where(x => x.Title == entity.Title);
                }
                if (entity.Price != null)
                {
                    filteredGames = filteredGames.Where(x => x.Price == entity.Price);
                }
                if (!string.IsNullOrEmpty(entity.Description))
                {
                    filteredGames = filteredGames.Where(x => x.Description == entity.Description);
                }
                if (entity.Rate != null)
                {
                    filteredGames = filteredGames.Where(x => x.Rate == entity.Rate);
                }
                if (!string.IsNullOrEmpty(entity.ImageUrl))
                {
                    filteredGames = filteredGames.Where(x => x.ImageUrl == entity.ImageUrl);
                }
                if (entity.UserId != null && entity.UserId != Guid.Empty)
                {
                    filteredGames = filteredGames.Where(x => x.UserId == entity.UserId);
                }
            }
            catch
            {
                throw new Exception("Error occured while filtering object");
            }


            return filteredGames;
        }

        public async Task<Game> GetDbObjectByIdAsync(Guid id)
        {
            using (DBContext context = new DBContext())
            {
                return await context.Games.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Game does not exists!");
            }
        }

        public async Task<Game> UpdateDbObjectAsync(Guid id, Game entity)
        {
            using (DBContext context = new DBContext())
            {
                Game game = await context.Games.FirstOrDefaultAsync(x => x.Id == id);

                if (game == null)
                {
                    return await AddDbObjectAsync(entity);
                }

                if (game.Title != null)
                {
                    game.Title = entity.Title;
                }

                if (entity.Price != null)
                {
                    game.Price = entity.Price;
                }

                if (entity.Description != null)
                {
                    game.Description = entity.Description;
                }

                if (entity.Rate != null)
                {
                    game.Rate = entity.Rate;
                }

                if (entity.UserId != null)
                {
                    User user = await context.Users.Include(u => u.Games).FirstOrDefaultAsync(x => x.Id == entity.UserId) ?? throw new Exception("User with this id does not exists!");

                    game.UserId = user.Id;

                    if (user.Games == null)
                    {
                        user.Games = new List<Game>();
                    }

                    Game currGame = user.Games.FirstOrDefault(x => x.Id == game.Id);

                    if (currGame == null)
                    {
                        user.Games.Add(game);
                    }
                }

                await context.SaveChangesAsync();
                return game;
            }
        }
    }
}
