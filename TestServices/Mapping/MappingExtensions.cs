﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestData.DbModels;
using TestData.Repositories.GameRepository;
using TestData.Repositories.UserRepository;
using TestServices.DbService;
using TestServices.Models.Game;
using TestServices.Models.User;

namespace TestServices.Mapping
{
    public static class MappingExtensions
    {

        #region GameMappings

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

        #endregion


        #region UserMappings

        public static User MapUserRequestModelToUser(this UserRequestModel model)
        {
            User user = new User();

            List<Game> games = new List<Game>();

            if (model.Game_Ids != null)
            {
                foreach (Guid id in model.Game_Ids)
                {
                    games.Add(new Game { Id = id });
                }
            }

            user.UserName = model.UserName;
            user.Status = model.Status;
            user.Balance = model.Balance;
            user.Email = model.Email;
            user.Password = model.Password;
            user.Games = games;

            return user;
        }

        public static UserResponseModel MapUserToUserResponseModel(this User user)
        {
            UserResponseModel model = new UserResponseModel();

            List<GameModel> gameModels = new List<GameModel>();

            if (user.Games != null)
            {
                foreach (Game game in user.Games)
                {
                    gameModels.Add(MapToGameModel(game));
                }
            }

            model.UserName = user.UserName;
            model.Status = user.Status;
            model.Balance = user.Balance;
            model.Email = user.Email;
            model.Games = gameModels;


            return model;
        }

        #endregion
    }
}