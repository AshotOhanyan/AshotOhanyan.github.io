using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestServices.ServiceConstants
{
    public static class AuthOptions
    {
        public static readonly string ISSUER = Environment.GetEnvironmentVariable("ISSUER", EnvironmentVariableTarget.Machine);
        public static readonly string AUDIENCE = Environment.GetEnvironmentVariable("AUDIENCE", EnvironmentVariableTarget.Machine);

        private static readonly string KEY = Environment.GetEnvironmentVariable("KEY", EnvironmentVariableTarget.Machine);
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
