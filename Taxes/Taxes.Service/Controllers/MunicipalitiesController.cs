using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Taxes.Service.BusinessLogic;
using Taxes.Service.DataLayer.Models;
using Taxes.Service.DataLayer.Repositories;
using Taxes.Service.Exceptions;

namespace Taxes.Service.Controllers
{
    public class MunicipalitiesController : BaseController<Municipality>
    {
        private readonly IMunicipalityRepository _municipalityRepository;

        public MunicipalitiesController(IMunicipalityRepository repository) : base(repository)
        {
            _municipalityRepository = repository;
        }

        [HttpGet]
        [EnableQuery]
        [ODataRoute("MunicipalityWithTax")]
        public IActionResult GetWithTax([FromODataUri]string name, [FromODataUri]DateTime date)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Municipality name cannot be empty");
            }

            // If no date is provided, date is automatically set to DateTime.MinValue
            if (date == DateTime.MinValue)
            {
                return BadRequest("Date was not provided or it is not valid");
            }

            IEnumerable<Municipality> municipalities;

            try
            {
                municipalities = _municipalityRepository.FindByName(name).ToList();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }

            if (!municipalities.Any())
            {
                return NotFound("No municipalities have been found with the provided name");
            }

            return Ok(municipalities.Select(x => TaxCalculator.CalculateTax(x, date)));
        }
    }
}
