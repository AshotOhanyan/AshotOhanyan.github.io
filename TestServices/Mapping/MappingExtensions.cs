using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestData.DbModels;
using TestServices.Models;

namespace TestServices.Mapping
{
    public static class MappingExtensions
    {
        public static GameModel MapToGameModel(this Game game)
        {
            GameModel gameModel = new GameModel();

            gameModel.Title = game.Title;
            gameModel.Price = game.Price;
            gameModel.Description = game.Description;
            gameModel.Rate = game.Rate;
            gameModel.ImageUrl = game.ImageUrl;
            gameModel.UserId = game.UserId;

            return gameModel;
        }

        public static Game MapToGame(this GameModel model)
        {
            Game game = new Game();


            game.Title = model.Title;
            game.Price = model.Price;
            game.Description = model.Description;
            game.Rate = model.Rate;
            game.ImageUrl = model.ImageUrl;
            game.UserId = model.UserId; 

            return game;
        }
    }
}
