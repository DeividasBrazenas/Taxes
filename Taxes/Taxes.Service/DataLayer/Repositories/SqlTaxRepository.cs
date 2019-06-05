using Taxes.Service.DataLayer.Models;

namespace Taxes.Service.DataLayer.Repositories
{
    public class SqlTaxRepository : SqlBaseRepository<Tax>, ITaxRepository
    {
        public SqlTaxRepository(TaxesContext context) : base(context)
        {
        }
    }
}