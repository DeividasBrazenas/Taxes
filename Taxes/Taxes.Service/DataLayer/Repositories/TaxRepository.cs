using Taxes.Service.DataLayer.Models;

namespace Taxes.Service.DataLayer.Repositories
{
    public class TaxRepository : BaseRepository<Tax>, ITaxRepository
    {
        public TaxRepository(TaxesContext context) : base(context)
        {
        }
    }
}