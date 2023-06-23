using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestData.DbModels;

namespace TestData.Repositories.UserRepository
{
    public interface IUserRepository : IBaseRepository<User>
    {
    }
}
