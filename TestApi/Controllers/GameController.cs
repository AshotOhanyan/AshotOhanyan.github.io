using Microsoft.AspNetCore.Mvc;
using TestData.DbModels;
using TestData.Repositories.GameRepository;

namespace TestApi.Controllers
{
    [ApiController]
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
        public async Task<Game> AddGameAsync(Game game)
        {
            await _repo.AddDbObjectAsync(game);
            return game;
        }

        [HttpPut]
        public async Task<Game> UpdateGameAsync(Guid id,Game game)
        {
            await _repo.UpdateDbObjectAsync(id, game);
            return game;
        }

        [HttpDelete]
        public async Task RemoveGame(Guid id)
        {
            await _repo.DeleteDbObjectAsync(id);
        }
    }
}
