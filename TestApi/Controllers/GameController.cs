using Microsoft.AspNetCore.Mvc;
using TestServices.Models;
using TestServices.Models.Game;
using TestServices.Services.GameService;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _service;

        public GameController(IGameService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<List<GameModel>> GetAllGamesAsync()
        {
            IEnumerable<GameModel> games = await _service.GetAllDbObjectsAsync();
            return games.ToList();
        }

        [HttpPost]
        public List<GameModel> GetGamesByFilterAsync(GameModel model)
        {
            IEnumerable<GameModel> games = _service.GetAllDbObjectsByFilterAsync(model);
            return games.ToList();
        }

        [HttpPost]
        public async Task<GameModel> GetGameByIdAsync(Guid id)
        {
            return await _service.GetDbObjectByIdAsync(id);
        }

        [HttpPost]
        public async Task<GameModel> AddGameAsync(GameModel model)
        {
            return await _service.AddDbObjectAsync(model);
            
        }

        [HttpPut]
        public async Task<GameModel> UpdateGameAsync(Guid id, GameModel model)
        {
            return await _service.UpdateDbObjectAsync(id, model);
             
        }

        [HttpDelete]
        public async Task RemoveGame(Guid id)
        {
            await _service.DeleteDbObjectAsync(id);
        }
    }
}
