using Microsoft.AspNetCore.Mvc;
using TestData.DbModels;
using TestData.Repositories.UserRepository;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repo;

        public UserController(IUserRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<List<User>> GetAllUsersAsync()
        {
            IEnumerable<User> Users = await _repo.GetAllDbObjectsAsync();
            return Users.ToList();
        }

        [HttpPost]
        public List<User> GetUsersByFilterAsync(User User)
        {
            IQueryable<User> Users = _repo.GetAllDbObjectsByFilterAsync(User);
            return Users.ToList();
        }

        [HttpPost]
        public async Task<User> GetUserByIdAsync(Guid id)
        {
            return await _repo.GetDbObjectByIdAsync(id);
        }

        [HttpPost]
        public async Task<User> AddUserAsync(User User)
        {
            return await _repo.AddDbObjectAsync(User);

        }

        [HttpPut]
        public async Task<User> UpdateUserAsync(Guid id, User User)
        {
            return await _repo.UpdateDbObjectAsync(id, User);

        }

        [HttpDelete]
        public async Task RemoveUser(Guid id)
        {
            await _repo.DeleteDbObjectAsync(id);
        }
    }
}
