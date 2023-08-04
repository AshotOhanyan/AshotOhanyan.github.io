using Azure.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TestData.DbConstants.UserConstants;
using TestData.DbModels;
using TestData.Exceptions;
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

        #region CRUD
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

        #endregion


        public async Task<UserSignUpResponseModel> SignUpAsync(UserSignUpModel model)
        {

            if (model.Password == null || model.ConfirmPassword == null || model.Password.Trim() != model.ConfirmPassword.Trim())
            {
                throw new SignUpFailedException("Password or ConfirmPassword fields are incorrect!");
            }


            var emailValidationToken = TokenGenerator.GenerateEightCharacterToken();

            User user = new User()
            {
                UserName = model.UserName ?? throw new SignUpFailedException("UserName not valid!"),
                Email = model.Email ?? throw new SignUpFailedException("Email not valid!"),
                Password = model.Password ?? throw new SignUpFailedException("Password not valid!"),
                ConfirmationToken = emailValidationToken ?? throw new SignUpFailedException("ConfirmationToken not valid!"),
                RoleId = RoleConstants.Default
            };

            try
            {
                Task sendEmail = Task.Factory.StartNew(() => EmailConfirmation.SendEmail(user.Email, "Email Validation Code", emailValidationToken));

                user.TokenExpirationDate = DateTime.UtcNow.AddDays(4);

                sendEmail.Wait();

                user = await _repo.AddDbObjectAsync(user);

                return new UserSignUpResponseModel() { Id = user.Id };
            }
            catch
            {
                throw new SignUpFailedException("Email not valid!");
            }
        }


        public async Task ConfirmEmailToken(Guid userId, string token)
        {
            token = token.Trim();

            if (string.IsNullOrEmpty(token))
            {
                throw new SignUpFailedException("Email not valid!");
            }

            User? user = await _repo.GetAllDbObjectsByFilterAsync(new User { ConfirmationToken = token }).FirstOrDefaultAsync();

            if (user == null)
            {
                await _repo.DeleteDbObjectAsync(userId);

                throw new SignUpFailedException("Wrong Token!");
            }

            TimeSpan time = DateTime.UtcNow.Subtract(user.TokenExpirationDate.Value);

            if (time >= TimeSpan.Zero)
            {
                user.ConfirmationToken = null;
                user.IsEmailConfirmed = false;

                await _repo.SaveChangesAsync();

                throw new SignUpFailedException("Token has expired!");
            }


            await _repo.UpdateDbObjectAsync(user.Id, new User { IsEmailConfirmed = true });
        }

        public async Task<string> SignInAsync(UserSignInRequestModel model)
        {
            string accessToken;

            if (string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.UserNameOrEmail))
            {
                throw new SignInFailedException("Username,Email and Password can not be empty!");
            }
            User? user = new User();

            if (model.UserNameOrEmail.Contains("@"))
            {
                string email = model.UserNameOrEmail;

                user = await _repo.GetAllDbObjectsByFilterAsync(new User { Email = email }).FirstOrDefaultAsync();
            }
            else
            {
                string userName = model.UserNameOrEmail;

                user = await _repo.GetAllDbObjectsByFilterAsync(new User { UserName = userName }).FirstOrDefaultAsync();
            }

            if (user == null)
            {
                throw new SignInFailedException("User with this email does not exists!");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password!.Trim(), UserInfo.Salt);

            if (passwordHash != user.Password!.Trim())
            {
                throw new SignInFailedException("Wrong Password!");
            }


            try {

                if(string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Email))
                {
                    throw new SignInFailedException("Username and Email can not be empty!");
                }

                string role = string.IsNullOrEmpty(user.Role!.Name) ? throw new SignInFailedException("Role can not be null or empty!") : user.Role.Name;

                accessToken = GenerateAccessToken(user.Id.ToString(), user.UserName, user.Email,role);
                string refreshToken = GenerateRefreshToken();

                await _repo.UpdateDbObjectAsync(user.Id, new User { RefreshToken = refreshToken });
                
            }
            catch
            {
                throw new SignInFailedException("Error while creating jwt token!");
            }


            return accessToken;
        }

        public string GenerateAccessToken(string id,string name,string email,string role)
        {
            Random r = new Random();

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,id),
                new Claim(ClaimTypes.Name,name),
                new Claim(ClaimTypes.Email,email),
                new Claim("tokenId",r.Next().ToString()),
                new Claim(ClaimsIdentity.DefaultRoleClaimType,role)
            };

            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(15)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
        

        public string GenerateRefreshToken()
        {
            Random r = new Random();
            var secretKey = AuthOptions.GetSymmetricSecurityKey();

            List<Claim> claims = new List<Claim>
            {
                new Claim("IsRefreshToken","true"),
                new Claim("tokenId",r.Next().ToString())
            };

            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromDays(7)),
                signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)
            );

            var refreshToken = new JwtSecurityTokenHandler().WriteToken(jwt);

            return refreshToken;
        }

        public async Task ResetPassword(Guid userId, string password, string confirmPassword)
        {
            User user = await _repo.GetDbObjectByIdAsync(userId);
           
            
            if (user == null)
                throw new ArgumentNullException(userId.ToString(), "User with this Id does not exists!");

            if(password.Trim() != confirmPassword.Trim())
                throw new OperationFailedException("Password and Confirm Password must be same!");

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password!.Trim(), UserInfo.Salt);

           if(user.Password == passwordHash)
            {
                throw new OperationFailedException("New password can not be same as previus one!");
            }

          await _repo.UpdateDbObjectAsync(userId, new User { Password = passwordHash});
        }
    }
}
