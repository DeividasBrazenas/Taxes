using System;
using System.Linq;
using Taxes.Service.DataLayer.ExModels;
using Taxes.Service.DataLayer.Models;
using Taxes.Service.Exceptions;

namespace Taxes.Service.BusinessLogic
{
    public class TaxCalculator
    {
        public static MunicipalityWithTax CalculateTax(Municipality municipality, DateTime date)
        {
            if (municipality.Taxes == null || municipality.Taxes.Count == 0)
            {
                throw new NotFoundException("Municipality '" + municipality.Name + "' does not have any taxes on the provided date " + date);
            }

            var orderedTaxes = municipality.Taxes.OrderBy(x => (x.EndDate - x.StartDate));

            return new MunicipalityWithTax(municipality, orderedTaxes.FirstOrDefault());
        }
    }
}
