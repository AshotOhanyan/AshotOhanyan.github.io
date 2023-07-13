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
    }
}
