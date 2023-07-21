using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestServices.Models.User
{
    public class UserSignInRequestModel
    {
        public string? UserNameOrEmail { get; set; }
        public string? Password { get; set; }
    }
}
