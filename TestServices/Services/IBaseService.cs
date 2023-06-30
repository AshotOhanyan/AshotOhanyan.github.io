using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestServices.Services
{
    public interface IBaseService<T>
    {
        Task<IEnumerable<T>> GetAllDbObjectsAsync();
        IEnumerable<T> GetAllDbObjectsByFilterAsync(T model);
        Task<T> GetDbObjectByIdAsync(Guid id);
        Task<T> UpdateDbObjectAsync(Guid id, T model);
        Task<T> AddDbObjectAsync(T model);
        Task DeleteDbObjectAsync(Guid id);
    }
}
