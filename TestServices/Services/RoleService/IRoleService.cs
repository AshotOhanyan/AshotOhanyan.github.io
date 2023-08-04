using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestServices.Models.Role;

namespace TestServices.Services.RoleService
{
    public interface IRoleService
    {
        public Task AddRoleAsync(RoleModel model);
        public Task RemoveRoleAsync(Guid id);
        public Task<RoleModel> GetRoleByIdAsync(Guid id);
        public Task<IEnumerable<RoleModel>> GetAllRolesAsync();
    }
}
