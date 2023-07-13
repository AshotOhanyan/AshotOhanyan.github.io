using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestServices.Services
{
    public interface IBaseService<T,Y>
    {
        Task<IEnumerable<Y>> GetAllDbObjectsAsync();
        IEnumerable<Y> GetAllDbObjectsByFilterAsync(T model);
        Task<Y> GetDbObjectByIdAsync(Guid id);
        Task<Y> UpdateDbObjectAsync(Guid id, T model);
        Task<Y> AddDbObjectAsync(T model);
        Task DeleteDbObjectAsync(Guid id);
    }
}
