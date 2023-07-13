using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestServices.Models.Game;

namespace TestServices.Models.User
{
    public class UserSignUpResponseModel
    {
        public Guid? UserId { get; set; }

        public string? EmailConfirmationToken { get; set; }

    }
}
