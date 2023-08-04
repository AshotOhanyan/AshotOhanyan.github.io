using Azure.Core;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using TestData.DbModels;
using TestServices.Models.Role;
using TestServices.Models.User;
using TestServices.OtherServices;
using TestServices.Services.RoleService;
using TestServices.Services.UserService;

namespace TestApi.Middlewares
{
    public class JwtRefreshMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public JwtRefreshMiddleware(RequestDelegate next, IUserService userService,IRoleService roleService)
        {
            _next = next;
            _userService = userService;
            _roleService = roleService;
        }

        public async Task InvokeAsync(HttpContext context)
         {
            Console.WriteLine($"Request Path: {context.Request.Path}");

            await _next(context);

            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                context.Request.Headers.TryGetValue("Authorization", out var accessKey);

                if (accessKey.ToString().StartsWith("Bearer", StringComparison.OrdinalIgnoreCase))
                {
                    accessKey = accessKey.ToString().Substring("Bearer ".Length);
                }

                bool isAccessTokenExpired = TokenGenerator.IsTokenExpired(accessKey!);


                if (isAccessTokenExpired)
                {
                    UserResponseModel? model = _userService.GetAllDbObjectsByFilterAsync(new UserRequestModel { UserName = context.User.Identity!.Name }).FirstOrDefault();

                    RoleModel role = await _roleService.GetRoleByIdAsync(model!.RoleId!.Value); 

                    string? refreshToken = model!.RefreshToken;

                    bool isRefreshTokenExpired = TokenGenerator.IsTokenExpired(refreshToken!);

                    if (isRefreshTokenExpired)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        await context.Response.WriteAsync("Refresh token has expired!");
                        await _next(context);
                    }

                    string token = _userService.GenerateAccessToken(model.Id.ToString()!, model.UserName!, model.Email!,role.Name!);

                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    await context.Response.WriteAsync(token);

                    await _next(context);
                }
            }
        }
    }
}
