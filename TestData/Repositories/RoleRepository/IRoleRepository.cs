using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestData.DbModels;

namespace TestData.Repositories.RoleRepository
{
    public interface IRoleRepository
    {
        public Task AddRoleAsync(Role role);
        public Task RemoveRoleAsync(Guid roleId);
        public Task<IEnumerable<Role>> GetAllRolesAsync();
        public Task<Role> GetRoleByIdAsync(Guid id);
    }
}
