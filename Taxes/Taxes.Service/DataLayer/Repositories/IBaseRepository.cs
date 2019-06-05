using System.Collections.Generic;
using System.Threading.Tasks;

namespace Taxes.Service.DataLayer.Repositories
{
    public interface IBaseRepository<T> where T : IEntity
    {
        IEnumerable<T> Get();
        T FindById(int id);
        Task<T> Add(T entity);
        Task<int> Delete(int id);
        Task<T> Update(T entity);
    }
}