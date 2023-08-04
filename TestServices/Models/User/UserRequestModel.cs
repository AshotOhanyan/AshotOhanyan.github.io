using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TestServices.Models.User
{
    public class UserRequestModel
    {
        public string? UserName { get; set; }
        public decimal? Balance { get; set; }
        public string? Status { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? ConfirmationToken { get; set; }

        [IgnoreDataMember]
        public DateTime? TokenExpirationDate { get; set; }

        public bool? IsEmailConfirmed { get; set; }

        [IgnoreDataMember]
        public string? RefreshToken { get; set; }

        [IgnoreDataMember]
        public Guid? RoleId{ get; set; }

        public List<Guid>? Game_Ids { get; set; }
    }
}
