﻿using Microsoft.AspNetCore.Mvc;
using System.Text;
using TestData.DbModels;
using TestData.Repositories.UserRepository;
using TestServices.Models;
using TestServices.Models.User;
using TestServices.Services.UserService;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }


        #region CRUD

        [HttpGet]
        public async Task<List<UserResponseModel>> GetAllUsersAsync()
        {
            IEnumerable<UserResponseModel> users = await _service.GetAllDbObjectsAsync();
            return users.ToList();
        }

        [HttpPost]
        public List<UserResponseModel> GetUsersByFilterAsync(UserRequestModel model)
        {
            IEnumerable<UserResponseModel> users = _service.GetAllDbObjectsByFilterAsync(model);
            return users.ToList();
        }

        [HttpPost]
        public async Task<UserResponseModel> GetUserByIdAsync(Guid id)
        {
            return await _service.GetDbObjectByIdAsync(id);
        }

        [HttpPost]
        public async Task<UserResponseModel> AddUserAsync(UserRequestModel model)
        {
            return await _service.AddDbObjectAsync(model);

        }

        [HttpPut]
        public async Task<UserResponseModel> UpdateUserAsync(Guid id, UserRequestModel model)
        {
            return await _service.UpdateDbObjectAsync(id, model);

        }

        [HttpDelete]
        public async Task RemoveUser(Guid id)
        {
            await _service.DeleteDbObjectAsync(id);
        }

        #endregion


        [HttpPost]
        public async Task<UserSignUpResponseModel> SignUpAsync(UserSignUpModel model)
        {

            
        }

        [HttpPost]
        public string GetToken(string token)
        {
            return token.Trim();
        }
    }
}
