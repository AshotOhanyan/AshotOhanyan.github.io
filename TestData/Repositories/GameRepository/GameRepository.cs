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
                if (game != null)
                {
                    throw new Exception($"{game} already exists!");
                }

                try
                {
                    await context.AddAsync(entity);
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
            using(DBContext context = new DBContext())
            {
                Game game = await context.Games.FirstOrDefaultAsync(x => x.Id == id);
                if (game == null)
                {
                    throw new Exception($"{game} does not exists!");
                }

                try
                {
                    context.Remove(game);
                }
                catch
                {
                    throw new Exception("Error while removing game from database!");
                }
            }
        }

        public async Task<IEnumerable<Game>> GetAllDbObjectsAsync()
        {
            using(DBContext context = new DBContext())
            {
                return await context.Games.ToListAsync();
            }
        }

        public async Task<IQueryable<Game>> GetAllDbObjectsByFilterAsync(Game entity)
        {
            
            using (DBContext context = new DBContext())
            {
                IQueryable<Game> filteredGames = context.Games.AsQueryable();

                if (!string.IsNullOrEmpty(entity.Title))
                {
                    filteredGames = filteredGames.Where(x => x.Title == entity.Title);
                }

                if(entity.Price != null)
                {

                }
            }
        }

        public Task<Game> GetDbObjectByIdAsync(dynamic id)
        {
            throw new NotImplementedException();
        }

        public Task<Game> UpdateDbObjectAsync(dynamic id, Game entity)
        {
            throw new NotImplementedException();
        }
    }
}
