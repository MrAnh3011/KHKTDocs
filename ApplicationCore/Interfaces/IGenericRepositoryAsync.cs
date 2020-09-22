using ApplicationCore.Entities.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IGenericRepositoryAsync<T> where T : BaseEntity, IAggregateRoot
    {
        Task<T> GetByIdAsync(string id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<int> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(string id);
        Task<IEnumerable<T>> SelectQuery(string where);
    }
}
