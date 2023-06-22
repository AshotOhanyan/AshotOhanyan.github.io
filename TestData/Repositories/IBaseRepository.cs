using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestData.Repositories
{
    public interface IBaseRepository<T>
    {
        Task<IEnumerable<T>> GetAllDbObjectsAsync();
        Task<IQueryable<T>> GetAllDbObjectsByFilterAsync(T entity);
        Task<T> GetDbObjectByIdAsync(Guid id);
        Task<T> UpdateDbObjectAsync(Guid id,T entity);
        Task<T> AddDbObjectAsync(T entity);
        Task DeleteDbObjectAsync(Guid id);
    }
}
