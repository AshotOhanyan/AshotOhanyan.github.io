using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestData.Data;
using TestData.DbConstants.UserConstants;
using TestData.DbModels;
using TestData.Exceptions;

namespace TestData.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly DBContext dbContext;
        private readonly IMemoryCache cache;

        public UserRepository(DBContext dbContext, IMemoryCache cache)
        {
            this.dbContext = dbContext;
            this.cache = cache;
        }

        public async Task<User> AddDbObjectAsync(User entity)
        {
            using (DBContext context = new DBContext())
            {
                try
                {

                    User user = new User
                    {
                        Id = Guid.NewGuid(),
                        UserName = string.IsNullOrEmpty(entity.UserName) ? throw new ArgumentNullException(entity.UserName, "UserName can not be empty!") : entity.UserName,
                        Email = string.IsNullOrEmpty(entity.Email) ? throw new ArgumentNullException(entity.Email, "Email can not be empty!") : entity.Email,
                        Password = string.IsNullOrEmpty(entity.Password) ? throw new ArgumentNullException(entity.Password, "Password can not be empty!") : BCrypt.Net.BCrypt.HashPassword(entity.Password.Trim(),UserInfo.Salt),
                        Balance = entity.Balance ?? 0,
                        IsEmailConfirmed = entity.IsEmailConfirmed ?? false,
                        ConfirmationToken = entity.ConfirmationToken,
                        TokenExpirationDate = entity.TokenExpirationDate,
                        RefreshToken = entity.RefreshToken,
                        Status = UserStatus.Active,
                        Games = new List<Game>()
                    };


                    if (entity.Games != null && entity.Games.Any())
                    {
                        foreach (Game game in entity.Games)
                        {
                            if (game != null)
                            {
                                Game? currGame = await context.Games.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == game.Id);

                                if (currGame != null && currGame.UserId == null)
                                {
                                    user.Games.Add(currGame);
                                    currGame.UserId = user.Id;
                                }
                            }
                        }
                    }

                    cache.Set(user.Id, user, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(15)));

                    await context.AddAsync(user);
                    await context.SaveChangesAsync();

                    return user;
                }
                catch
                {
                    throw new OperationFailedException(entity, "Error occured while creating User");
                }
            }
        }

        public async Task ChangePassword(Guid userId, string password)
        {
            using (DBContext context = new DBContext())
            {
                try
                {
                    User? currUser = await context.Users.FirstOrDefaultAsync(x => x.Id == userId) ?? throw new ArgumentNullException(userId.ToString(), "User with this id does not exist!");
                    if (string.IsNullOrEmpty(password))
                    {
                        throw new ArgumentNullException(password, "Password can not be empty!");
                    }
                    currUser.Password = BCrypt.Net.BCrypt.HashPassword(password.Trim(), UserInfo.Salt);

                    await context.SaveChangesAsync();
                }
                catch
                {
                    throw new OperationFailedException("Error while changing the password!");
                }
            }
        }

        public async Task DeleteDbObjectAsync(Guid id)
        {
            using (DBContext context = new DBContext())
            {
                cache.TryGetValue(id, out User? user);

                if (user == null)
                {
                    user = await context.Users.FirstOrDefaultAsync(x => x.Id == id) ?? throw new OperationFailedException("User does not exists!");
                }

                try
                {
                    cache.Remove(user.Id);
                    context.Remove(user);
                    await context.SaveChangesAsync();
                }
                catch
                {
                    throw new OperationFailedException(user, "Error while removing User from database!");
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
                if (entity.Balance != null)
                {
                    filteredUsers = filteredUsers.Where(x => x.Balance == entity.Balance);
                }
                if (!string.IsNullOrEmpty(entity.Password))
                {
                    filteredUsers = filteredUsers.Where(x => x.Password == entity.Password);
                }
                if (!string.IsNullOrEmpty(entity.Status))
                {
                    filteredUsers = filteredUsers.Where(x => x.Status == entity.Status);
                }
                if(!string.IsNullOrEmpty(entity.ConfirmationToken))
                {
                    filteredUsers = filteredUsers.Where(x => x.ConfirmationToken == entity.ConfirmationToken);
                }
                if(entity.TokenExpirationDate != null)
                {
                    filteredUsers = filteredUsers.Where(x => x.TokenExpirationDate == entity.TokenExpirationDate);
                }
                if(entity.IsEmailConfirmed != null) 
                {
                    filteredUsers = filteredUsers.Where(x => x.IsEmailConfirmed == entity.IsEmailConfirmed);
                }
                if (entity.RefreshToken != null)
                {
                    filteredUsers = filteredUsers.Where(x => x.RefreshToken == entity.RefreshToken);
                }
            }
            catch
            {
                throw new OperationFailedException(entity, "Error occured while filtering object");
            }


            return filteredUsers;
        }

        public async Task<User> GetDbObjectByIdAsync(Guid id)
        {
            using (DBContext context = new DBContext())
            {
                cache.TryGetValue(id, out User? dbObject);
                if (dbObject != null) return dbObject;

                User result = await context.Users.Include(x => x.Games).FirstOrDefaultAsync(x => x.Id == id) ?? throw new OperationFailedException("User does not exists!");
                cache.Set(id, result, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                return result;
            }
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public async Task<User> UpdateDbObjectAsync(Guid id, User entity)
        {
            using (DBContext context = new DBContext())
            {
                cache.TryGetValue(id, out User? user);

                if (user == null)
                {
                    user = await context.Users.Include(x => x.Games).FirstOrDefaultAsync(x => x.Id == id);
                }

                if (user == null)
                {
                    return await AddDbObjectAsync(entity);
                }

                try
                {

                    if (entity.UserName != null)
                    {
                        user.UserName = entity.UserName;
                    }
                    if (entity.Balance != null)
                    {
                        user.Balance = entity.Balance;
                    }
                    if (entity.Status == UserStatus.Active || entity.Status == UserStatus.Blocked)
                    {
                        user.Status = entity.Status;
                    }
                    if (entity.Password != null)
                    {
                        user.Password = entity.Password;
                    }
                    if (entity.ConfirmationToken != null)
                    {
                        user.ConfirmationToken = entity.ConfirmationToken;
                    }
                    if (entity.TokenExpirationDate != null)
                    {
                        user.TokenExpirationDate = entity.TokenExpirationDate;
                    }
                    if (entity.IsEmailConfirmed != null)
                    {
                        user.IsEmailConfirmed = entity.IsEmailConfirmed;
                    }
                    if (entity.RefreshToken != null)
                    {
                        user.RefreshToken = entity.RefreshToken;
                    }
                    if (entity.Games != null && entity.Games.Any())
                    {
                        foreach (Game game in entity.Games)
                        {
                            Game? currGame = await context.Games.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == game.Id);

                            if (currGame == null)
                                continue;

                            currGame.UserId = user.Id;

                            if (user.Games == null)
                            {
                                user.Games = new List<Game>();
                            }

                            user.Games.Add(currGame);
                        }
                    }


                    await context.SaveChangesAsync();
                    cache.CreateEntry(user);

                    return user;
                }
                catch
                {
                    throw new OperationFailedException(entity, "Error while updating user info!");
                }
            }
        }
    }
}
