using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestData.Data;
using TestData.DbModels;

namespace TestData.Repositories.RoleRepository
{
    public class RoleRepository : IRoleRepository
    {
        public async Task AddRoleAsync(Role role)
        {
            using (DBContext dbContext = new DBContext())
            {
                Role newRole = new Role()
                {
                    Id = Guid.NewGuid(),
                    Name = string.IsNullOrEmpty(role.Name) ? throw new ArgumentNullException(role.Name, "Name can not be empty!") : role.Name,
                };

                await dbContext.Roles.AddAsync(newRole);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            using (DBContext dbContext = new DBContext())
            {
                return await dbContext.Roles.ToListAsync();
            }
        }

        public async Task<Role> GetRoleByIdAsync(Guid id)
        {
            using (DBContext dbContext = new DBContext())
            {
                Role? result = await dbContext.Roles.FirstOrDefaultAsync(x => x.Id == id);

                if(result == null)
                {
                    throw new ArgumentNullException(id.ToString(), "Role with this id does not exists!");
                }

                return result;
            }
        }

        public async Task RemoveRoleAsync(Guid roleId)
        {
            using (DBContext dbContext = new DBContext())
            {
                Role? role = await dbContext.Roles.FirstOrDefaultAsync(x => x.Id == roleId);

                if (role != null)
                {
                    dbContext.Roles.Remove(role);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
