using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestServices.ServiceConstants;

namespace TestServices.OtherServices
{
    public static class TokenGenerator
    {
        public static string GenerateEightCharacterToken()
        {
            string allChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random r = new Random();

            return new string(
                Enumerable.Repeat(allChar, 8)
                .Select(token => token[r.Next(token.Length)]).ToArray()).ToString();
        }


        public static bool IsTokenExpired(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = AuthOptions.ISSUER,
                ValidateAudience = true,
                ValidAudience = AuthOptions.AUDIENCE,
                ValidateLifetime = true,
                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                ValidateIssuerSigningKey = true

            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken securityToken);

                if (securityToken.ValidTo < DateTime.UtcNow)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }catch (SecurityTokenExpiredException ex)
            {
                return true;
            }catch(System.ArgumentException ex)
            {
                return false;
            }
        }
    }
}
