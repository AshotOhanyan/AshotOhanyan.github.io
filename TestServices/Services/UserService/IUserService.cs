using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestServices.Models.User;

namespace TestServices.Services.UserService
{
    public interface IUserService : IBaseService<UserRequestModel,UserResponseModel>
    {
        public Task<UserSignUpResponseModel> SignUpAsync(UserSignUpModel model);
        public Task<string> SignInAsync(UserSignInRequestModel model);
        public string GenerateAccessToken(string id, string name, string email);
        public string GenerateRefreshToken();
        public Task ConfirmEmailToken(Guid userId, string token);
    }
}
