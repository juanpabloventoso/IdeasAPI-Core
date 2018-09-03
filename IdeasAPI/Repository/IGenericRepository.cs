using System.Linq;
using System.Threading.Tasks;

namespace IdeasAPI.Repository
{
    public interface IGenericRepository<T>
    {
        Task<T> GetAsync(string id);

        IQueryable<T> Query();

        Task InsertAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

    }
}