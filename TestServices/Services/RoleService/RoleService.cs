using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestData.DbModels;
using TestData.Repositories.RoleRepository;
using TestServices.Mapping;
using TestServices.Models.Role;

namespace TestServices.Services.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repo;

        public RoleService(IRoleRepository repo)
        {
            _repo = repo;
        }

        public async Task AddRoleAsync(RoleModel model)
        {
            await _repo.AddRoleAsync(model.MapRoleModelToRole());
        }

        public async Task<IEnumerable<RoleModel>> GetAllRolesAsync()
        {
            IEnumerable<Role> roles = await _repo.GetAllRolesAsync();
            List<RoleModel> result = new List<RoleModel>();

            foreach (Role role in roles)
            {
                result.Add(role.MapRoleToRoleModel());
            }

            return result;
        }

        public async Task<RoleModel> GetRoleByIdAsync(Guid id)
        {
            Role role = await _repo.GetRoleByIdAsync(id);

            return role.MapRoleToRoleModel();
        }

        public async Task RemoveRoleAsync(Guid id)
        {
            await _repo.RemoveRoleAsync(id);
        }
    }
}
