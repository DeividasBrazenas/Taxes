using System;
using System.Linq;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Taxes.Service.BusinessLogic;
using Taxes.Service.DataLayer;
using Taxes.Service.DataLayer.Models;

namespace Taxes.Service.Controllers
{
    public class MunicipalitiesController : BaseController<Municipality>
    {
        public MunicipalitiesController(TaxesContext context) : base(context)
        {
        }

        [HttpPost]
        [EnableQuery]
        [ODataRoute("MunicipalitiesWithTax")]
        public IActionResult GetWithTax([FromODataUri]DateTime date)
        {
            return Ok(Context.Municipalities.Include(x => x.Taxes).Select(x => TaxCalculator.CalculateTax(x, date)));
        }
    }
}
