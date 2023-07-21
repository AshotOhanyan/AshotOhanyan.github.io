using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestData.DbModels
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string? UserName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        public decimal? Balance { get; set; }

        [Required]
        public string? Status { get; set; }

        public string? ConfirmationToken { get; set; }

        public DateTime? TokenExpirationDate { get; set; }

        [Required]
        public bool? IsEmailConfirmed { get; set; }

        public string? RefreshToken { get; set; }

        public virtual List<Game>? Games { get; set; }
    }
}
