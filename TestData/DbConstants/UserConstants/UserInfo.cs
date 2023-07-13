using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestData.DbConstants.UserConstants
{
    public static class UserInfo
    {
        public readonly static string Salt = Environment.GetEnvironmentVariable("Salt", EnvironmentVariableTarget.Machine);
    }
}
