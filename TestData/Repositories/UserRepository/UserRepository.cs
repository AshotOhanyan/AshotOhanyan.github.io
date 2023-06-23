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
        public async Task<User> AddDbObjectAsync(User entity)
        {
            using(DBContext context = new DBContext())
            {
                User user = await context.Users.Include(x => x.Games).FirstOrDefaultAsync(x => x.Id == entity.Id);
                if (user != null) 
                {
                    throw new Exception("User already exists");
                }

                try
                {
                    if (string.IsNullOrEmpty(entity.UserName))
                    {
                        throw new Exception("UserName can not be empty!");
                    }

                    user = new User
                    {
                        UserName = entity.UserName,
                        Games = entity.Games
                    };

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

        public Task DeleteDbObjectAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAllDbObjectsAsync()
        {
            throw new NotImplementedException();
        }

        public IQueryable<User> GetAllDbObjectsByFilterAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetDbObjectByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<User> UpdateDbObjectAsync(Guid id, User entity)
        {
            throw new NotImplementedException();
        }
    }
}
