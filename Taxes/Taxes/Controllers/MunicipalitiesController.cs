using System;
using System.Linq;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
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

        [EnableQuery]
        [ODataRoute("MunicipalitiesWithTax")]
        public IActionResult GetWithTax(DateTime date)
        {
            return Ok(Context.Municipalities.Select(x => TaxCalculator.CalculateTax(x, date)));
        }
    }
}
