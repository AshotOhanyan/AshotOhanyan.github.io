using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestData.Data;
using TestData.DbModels;

namespace TestData.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly DBContext dbContext;

        public UserRepository(DBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<User> AddDbObjectAsync(User entity)
        {
            using (DBContext context = new DBContext())
            {
                User user = await context.Users.FirstOrDefaultAsync(x => x.Id == entity.Id);

                if (user != null)
                    throw new Exception("User already exists");

                try
                {
                    user = new User
                    {
                        Id = Guid.NewGuid(),
                        UserName = string.IsNullOrEmpty(entity.UserName) ? throw new Exception("UserName can not be empty!") : entity.UserName,
                        Games = new List<Game>()
                    };


                    if (entity.Games != null && entity.Games.Any())
                    {
                        foreach (Game game in entity.Games)
                        {
                            if (game != null)
                            {
                                Game currGame = await context.Games.FirstOrDefaultAsync(x => x.Id == game.Id);

                                if (currGame != null && currGame.UserId == null)
                                {
                                    user.Games.Add(currGame);
                                    currGame.UserId = user.Id;
                                }
                            }
                        }
                    }

                    await context.AddAsync(user);
                    await context.SaveChangesAsync();
                }
                catch
                {
                    throw new Exception("Error occured while creating User");
                }

                return user;
            }
        }

        public async Task DeleteDbObjectAsync(Guid id)
        {
            using (DBContext context = new DBContext())
            {
                User user = await context.Users.FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("User does not exists!");

                try
                {
                    context.Remove(user);
                    await context.SaveChangesAsync();
                }
                catch
                {
                    throw new Exception("Error while removing User from database!");
                }
            }
        }

        public async Task<IEnumerable<User>> GetAllDbObjectsAsync()
        {
            using (DBContext context = new DBContext())
            {
                return await context.Users.Include(g => g.Games).ToListAsync();
            }
        }

        public IQueryable<User> GetAllDbObjectsByFilterAsync(User entity)
        {
            IQueryable<User> filteredUsers;

            filteredUsers = dbContext.Users.Include(g => g.Games).AsQueryable();
            try
            {
                if (!string.IsNullOrEmpty(entity.UserName))
                {
                    filteredUsers = filteredUsers.Where(x => x.UserName == entity.UserName);
                }
            }
            catch
            {
                throw new Exception("Error occured while filtering object");
            }


            return filteredUsers;
        }

        public async Task<User> GetDbObjectByIdAsync(Guid id)
        {
            using (DBContext context = new DBContext())
            {
                return await context.Users.Include(x => x.Games).FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("User does not exists!");
            }
        }

        public async Task<User> UpdateDbObjectAsync(Guid id, User entity)
        {
            using (DBContext context = new DBContext())
            {
                User user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

                if (user == null)
                {
                    return await AddDbObjectAsync(entity);
                }

                user.UserName = entity.UserName ?? throw new Exception("UserName can not be null or empty!");

                await context.SaveChangesAsync();

                return user;
            }
        }
    }
}
