using Microsoft.AspNetCore.Mvc;
using TestServices.Models.Role;
using TestServices.Services.RoleService;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _service;

        public RoleController(IRoleService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<List<RoleModel>> GetAllRolesAsync()
        {
            IEnumerable<RoleModel> Roles = await _service.GetAllRolesAsync();
            return Roles.ToList();
        }


        [HttpPost]
        public async Task AddRoleAsync(RoleModel model)
        {
            await _service.AddRoleAsync(model);

        }


        [HttpGet]
        [Route("{id}")]
        public async Task<RoleModel> GetRoleByIdAsync(Guid id)
        {
            return await _service.GetRoleByIdAsync(id);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task RemoveRole(Guid id)
        {
            await _service.RemoveRoleAsync(id);
        }
    }
}
