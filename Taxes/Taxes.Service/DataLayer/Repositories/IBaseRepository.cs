using System.Collections.Generic;
using System.Threading.Tasks;
using Taxes.Service.DataLayer.Models;

namespace Taxes.Service.DataLayer.Repositories
{
    public interface IBaseRepository<T> where T : BaseModel
    {
        IEnumerable<T> Get();
        T FindById(int id);
        Task<T> Add(T entity);
        Task<int> Delete(int id);
        Task<T> Update(T entity);
    }
}