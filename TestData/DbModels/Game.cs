
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TestData.DbModels
{
    public class Game
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string? Title { get; set; }
        [Required]
        public decimal? Price { get; set; }
        [MaxLength(300)]
        public string? Description { get; set; }
       
        public float? Rate { get; set; }

        [MaxLength(100)]
        public string? ImageUrl { get; set; }

        public Guid? UserId { get; set; }
        public User? User { get; set; }
    }
}
