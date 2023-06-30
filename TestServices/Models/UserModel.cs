using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestServices.Models
{
    public class UserModel
    {
        public string? UserName { get; set; }

        public List<Guid>? Game_Ids { get; set; }
    }
}
