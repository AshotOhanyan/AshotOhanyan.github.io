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
        public async Task<Game> AddDbObjectAsync(Game entity)
        {
            using (DBContext context = new DBContext())
            {
                Game game = await context.Games.FirstOrDefaultAsync(x => x.Id == entity.Id);
                User currUser = new User();

                if (game != null)
                {
                    throw new Exception("Game already exists!");
                }

                try
                {

                    if (string.IsNullOrEmpty(entity.Title))
                    {
                        throw new Exception("Game most has a title!");
                    }
                    if (entity.Price == null)
                    {
                        throw new Exception("Game most has a price!");
                    }
                    if (entity.UserId != null && entity.UserId != Guid.Empty)
                    {
                        currUser = await context.Users.FirstOrDefaultAsync(x => x.Id == entity.UserId);
                        if (currUser != null)
                        {
                            entity.UserId = currUser?.Id;

                            if (currUser.Games == null)
                            {
                                currUser.Games = new List<Game>();
                            }
                        }
                    }

                    game = new Game
                    {
                        Title = entity.Title,
                        Price = entity.Price,
                        Description = entity.Description ?? "",
                        Rate = entity.Rate,
                        ImageUrl = entity.ImageUrl,
                        UserId = entity.UserId,
                    };

                    if (currUser != null && currUser.Games != null)
                    {
                        currUser.Games.Add(game);
                    }

                    await context.AddAsync(entity);
                    await context.SaveChangesAsync();
                }
                catch
                {
                    throw new Exception("Error while adding game to database!");
                }

                return entity;
            }
        }

        public async Task DeleteDbObjectAsync(Guid id)
        {
            using (DBContext context = new DBContext())
            {
                Game game = await context.Games.FirstOrDefaultAsync(x => x.Id == id);
                if (game == null)
                {
                    throw new Exception($"{game} does not exists!");
                }

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
                return await context.Games.ToListAsync();
            }
        }

        public IQueryable<Game> GetAllDbObjectsByFilterAsync(Game entity)
        {

            using (DBContext context = new DBContext())
            {
                IQueryable<Game> filteredGames = context.Games.AsQueryable();

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
        }

        public async Task<Game> GetDbObjectByIdAsync(Guid id)
        {
            using (DBContext context = new DBContext())
            {
                if (id == Guid.Empty)
                {
                    throw new Exception("Id can not be empty!");
                }

                Game game = await context.Games.FirstOrDefaultAsync(x => x.Id == id);

                if (game == null)
                {
                    throw new Exception("Game does not exists!");
                }

                return game;
            }
        }

        public async Task<Game> UpdateDbObjectAsync(Guid id, Game entity)
        {
            using (DBContext context = new DBContext())
            {
                if (id == Guid.Empty)
                {
                    throw new Exception("Id can not be empty!");
                }

                Game game = await context.Games.FirstOrDefaultAsync(x => x.Id == id);

                if (game == null)
                {
                    if (entity.UserId != null && entity.UserId != Guid.Empty)
                    {
                        User currUser = await context.Users.FirstOrDefaultAsync(x => x.Id == entity.UserId);
                        entity.UserId = currUser?.Id;
                    }

                    game = new Game()
                    {
                        Title = entity.Title,
                        Description = entity.Description,
                        Price = entity.Price,
                        Rate = entity.Rate,
                        UserId = entity.UserId,
                    };
                    await context.AddAsync(entity);
                    await context.SaveChangesAsync();
                    return entity;
                }

                game.Title = entity.Title;
                game.Price = entity.Price;
                game.Description = entity.Description;
                game.Rate = entity.Rate;
                game.UserId = entity.UserId;

                await context.SaveChangesAsync();
                return game;
            }
        }
    }
}
