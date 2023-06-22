using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestData.DbModels
{
    public class User
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        
        public virtual List<Game> Games { get; set; }
    }
}
