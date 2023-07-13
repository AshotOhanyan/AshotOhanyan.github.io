using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TestData.DbModels;
using TestData.Repositories.UserRepository;
using TestServices.Exceptions;
using TestServices.Mapping;
using TestServices.Models.User;
using TestServices.OtherServices;
using TestServices.ServiceConstants;

namespace TestServices.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<UserResponseModel> AddDbObjectAsync(UserRequestModel model)
        {
            User user = await _repo.AddDbObjectAsync(model.MapUserRequestModelToUser());

            return user.MapUserToUserResponseModel();
        }

        public async Task DeleteDbObjectAsync(Guid id)
        {
            await _repo.DeleteDbObjectAsync(id);
        }

        public async Task<IEnumerable<UserResponseModel>> GetAllDbObjectsAsync()
        {
            IEnumerable<User> users = await _repo.GetAllDbObjectsAsync();
            List<UserResponseModel> result = new List<UserResponseModel>();

            foreach (User user in users)
            {
                result.Add(user.MapUserToUserResponseModel());
            }

            return result;
        }

        public IEnumerable<UserResponseModel> GetAllDbObjectsByFilterAsync(UserRequestModel model)
        {
            IQueryable<User> filteredUsers = _repo.GetAllDbObjectsByFilterAsync(model.MapUserRequestModelToUser());
            List<UserResponseModel> result = new List<UserResponseModel>();

            foreach (User user in filteredUsers)
            {
                result.Add(user.MapUserToUserResponseModel());
            }

            return result;
        }

        public async Task<UserResponseModel> GetDbObjectByIdAsync(Guid id)
        {
            User User = await _repo.GetDbObjectByIdAsync(id);

            return User.MapUserToUserResponseModel();
        }

        public async Task<UserResponseModel> UpdateDbObjectAsync(Guid id, UserRequestModel model)
        {
            User User = await _repo.UpdateDbObjectAsync(id, model.MapUserRequestModelToUser());

            return User.MapUserToUserResponseModel();
        }

        public async Task<UserSignUpResponseModel> SignUpAsync(UserSignUpModel model)
        {

            if (model.Password == null || model.ConfirmPassword == null || model.Password.Trim() != model.ConfirmPassword.Trim())
            {
                throw new SignUpFailedException("Password or ConfirmPassword fields are incorrect!");
            }

            UserRequestModel user = new UserRequestModel
            {
                UserName = model.UserName,
                Password = model.Password,  
                Email = model.Email,
            };

            Random r = new Random();
            var x = r.Next(0, 1000000);
            string emailValidationToken = x.ToString("000000");



            try
            {

                EmailConfirmation.SendEmail(CompanyInfo.CompanyEmail, "Email Validation Code", emailValidationToken);

                Task task = new Task(() => {  })

                _repo.AddDbObjectAsync(user.MapUserRequestModelToUser());
            }
            catch
            {
                throw new Exception("Email not valid!");
            }


            return new UserSignUpResponseModel { EmailConfirmationToken = emailValidationToken };
        }


        public async string GetEmailToken()
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.PostAsync("url_of_other_action", null);
        }


    }
}
