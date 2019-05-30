using Taxes.Service.DataLayer;
using Taxes.Service.DataLayer.Models;

namespace Taxes.Service.Controllers
{
    public class TaxesController : BaseController<Tax>
    {
        public TaxesController(TaxesContext context) : base(context)
        {
        }
    }
}
