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

        [HttpGet]
        [EnableQuery]
        [ODataRoute("MunicipalityWithTax")]
        public IActionResult GetWithTax([FromODataUri]string name, [FromODataUri]DateTime date)
        {
            if (name == null)
            {
                return BadRequest("Municipality name cannot be null");
            }

            var municipalities = Context.Municipalities.Where(x => x.Name == name);

            if (!municipalities.Any())
            {
                return Ok("No municipalities have been found with the provided name");
            }

            return Ok(municipalities.Include(x => x.Taxes).Select(x => TaxCalculator.CalculateTax(x, date)));
        }
    }
}
