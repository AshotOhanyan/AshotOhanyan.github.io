using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestData.Data;
using TestData.DbModels;
using TestData.Exceptions;

namespace TestData.Repositories.GameRepository
{
    public class GameRepository : IGameRepository
    {
        private readonly DBContext dbContext;
        private readonly IMemoryCache cache;

        public GameRepository(DBContext dbContext, IMemoryCache cache)
        {
            this.dbContext = dbContext;
            this.cache = cache;
        }

        public async Task<Game> AddDbObjectAsync(Game entity)
        {
            using (DBContext context = new DBContext())
            {
                try
                {
                    Game game = new Game
                    {
                        Id = Guid.NewGuid(),
                        Title = string.IsNullOrEmpty(entity.Title) ? throw new ArgumentNullException(entity.Title, "Game most has a title!") : entity.Title,
                        Price = entity.Price ?? throw new ArgumentNullException(entity.Price.ToString(), "Game most has a price!"),
                        Description = entity.Description,
                        Rate = entity.Rate,
                        ImageUrl = entity.ImageUrl,
                        UserId = entity.UserId,
                    };

                    if (entity.UserId != null && entity.UserId != Guid.Empty)
                    {
                        User currUser = await context.Users.FirstOrDefaultAsync(x => x.Id == entity.UserId) ?? throw new ArgumentNullException(game.UserId.ToString(), "User does not exist!");

                        if (currUser.Games == null)
                        {
                            currUser.Games = new List<Game>();
                        }

                        currUser.Games.Add(game);
                        game.UserId = currUser.Id;

                    }

                    cache.Set(game.Id, game, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(15)));

                    await context.AddAsync(game);
                    await context.SaveChangesAsync();

                    return game;
                }
                catch
                {
                    throw new OperationFailedException(entity, "Error while adding game to database!");
                }
            }
        }

        public async Task DeleteDbObjectAsync(Guid id)
        {
            using (DBContext context = new DBContext())
            {
                cache.TryGetValue(id, out Game? game);
                if (game == null)
                {
                    game = await context.Games.FirstOrDefaultAsync(x => x.Id == id) ?? throw new OperationFailedException("Game already exists!");
                }

                try
                {
                    cache.Remove(game.Id);
                    context.Remove(game);
                    await context.SaveChangesAsync();
                }
                catch
                {
                    throw new OperationFailedException(game, "Error while removing game from database!");
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
                throw new OperationFailedException(entity, "Error occured while filtering object");
            }


            return filteredGames;
        }

        public async Task<Game> GetDbObjectByIdAsync(Guid id)
        {
            using (DBContext context = new DBContext())
            {
                cache.TryGetValue(id, out Game dbObject);
                if (dbObject != null) return dbObject;

                Game result = await context.Games.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id) ?? throw new OperationFailedException("Game does not exists!");
                cache.Set(id, result, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));

                return result;

            }
        }

        public async Task<Game> UpdateDbObjectAsync(Guid id, Game entity)
        {
            using (DBContext context = new DBContext())
            {

                cache.TryGetValue(id, out Game? game);

                if (game == null)
                {
                    game = await context.Games.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
                }

                if (game == null)
                {
                    return await AddDbObjectAsync(entity);
                }

                try
                {
                    if (entity.Title != null)
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
                        User user = await context.Users.Include(u => u.Games).FirstOrDefaultAsync(x => x.Id == entity.UserId) ?? throw new OperationFailedException("User with this id does not exists!");

                        game.UserId = user.Id;

                        if (user.Games == null)
                        {
                            user.Games = new List<Game>();
                        }

                        Game? currGame = user.Games.FirstOrDefault(x => x.Id == game.Id);

                        if (currGame == null)
                        {
                            user.Games.Add(game);
                        }
                    }

                    await context.SaveChangesAsync();
                    cache.CreateEntry(entity);
                    return game;
                }
                catch
                {
                    throw new OperationFailedException(entity, "Error while updating game info!");
                }
            }
        }
    }
}
