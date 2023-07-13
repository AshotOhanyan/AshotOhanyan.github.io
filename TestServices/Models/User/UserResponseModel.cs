using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestServices.Models.Game;

namespace TestServices.Models.User
{
    public class UserResponseModel
    {
        public string? UserName { get; set; }
        public decimal? Balance { get; set; }
        public string? Status { get; set; }
        public string? Email { get; set; }
        public List<GameModel>? Games { get; set; }
    }
}
