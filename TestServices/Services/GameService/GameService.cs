using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestData.DbModels;
using TestData.Repositories.GameRepository;
using TestServices.Mapping;
using TestServices.Models.Game;

namespace TestServices.Services.GameService
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _repo;

        public GameService(IGameRepository repo)
        {
            _repo = repo;
        }

        public async Task<GameModel> AddDbObjectAsync(GameModel model)
        {
            Game game = await _repo.AddDbObjectAsync(model.MapToGame());

            return game.MapToGameModel();
        }

        public async Task DeleteDbObjectAsync(Guid id)
        {
            await _repo.DeleteDbObjectAsync(id);
        }

        public async Task<IEnumerable<GameModel>> GetAllDbObjectsAsync()
        {
            IEnumerable<Game> games = await _repo.GetAllDbObjectsAsync();
            List<GameModel> result = new List<GameModel>();

            foreach (Game game in games)
            {
                result.Add(game.MapToGameModel());
            }

            return result;
        }

        public IEnumerable<GameModel> GetAllDbObjectsByFilterAsync(GameModel model)
        {
            IQueryable<Game> filteredGames = _repo.GetAllDbObjectsByFilterAsync(model.MapToGame());
            List<GameModel> result= new List<GameModel>();

            foreach(Game game in filteredGames)
            {
                result.Add(game.MapToGameModel()); 
            }

            return result;
        }

        public async Task<GameModel> GetDbObjectByIdAsync(Guid id)
        {
            Game game = await _repo.GetDbObjectByIdAsync(id);

            return game.MapToGameModel();
        }

        public async Task<GameModel> UpdateDbObjectAsync(Guid id, GameModel model)
        {
            Game game = await _repo.UpdateDbObjectAsync(id, model.MapToGame());

            return game.MapToGameModel();
        }
    }
}
