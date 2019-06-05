using Taxes.Service.DataLayer.Models;
using Taxes.Service.DataLayer.Repositories;

namespace Taxes.Service.Controllers
{
    public class TaxesController : BaseController<Tax>
    {
        public TaxesController(ITaxRepository repository) : base(repository)
        {
        }
    }
}
