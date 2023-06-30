using Microsoft.AspNetCore.Mvc;
using TestData.DbModels;
using TestData.Repositories.GameRepository;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class GameController : ControllerBase
    {
        private readonly IGameRepository _repo;

        public GameController(IGameRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<List<Game>> GetAllGamesAsync()
        {
            IEnumerable<Game> games = await _repo.GetAllDbObjectsAsync();
            return games.ToList();
        }

        [HttpPost]
        public List<Game> GetGamesByFilterAsync(Game game)
        {
            IQueryable<Game> games = _repo.GetAllDbObjectsByFilterAsync(game);
            return games.ToList();
        }

        [HttpPost]
        public async Task<Game> GetGameByIdAsync(Guid id)
        {
            return await _repo.GetDbObjectByIdAsync(id);
        }

        [HttpPost]
        public async Task<Game> AddGameAsync(Game game)
        {
            return await _repo.AddDbObjectAsync(game);
            
        }

        [HttpPut]
        public async Task<Game> UpdateGameAsync(Guid id,Game game)
        {
            return await _repo.UpdateDbObjectAsync(id, game);
             
        }

        [HttpDelete]
        public async Task RemoveGame(Guid id)
        {
            await _repo.DeleteDbObjectAsync(id);
        }
    }
}
