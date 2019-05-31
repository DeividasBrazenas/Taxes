using System;
using System.Collections.Generic;
using System.Linq;
using Taxes.Service.DataLayer.ExModels;
using Taxes.Service.DataLayer.Models;
using Taxes.Service.Extensions;

namespace Taxes.Service.BusinessLogic
{
    public class TaxCalculator
    {
        public static MunicipalityWithTax CalculateTax(Municipality municipality, DateTime date)
        {
            if (municipality == null)
            {
                throw new NotImplementedException();
            }

            if (date == DateTime.MinValue)
            {
                throw new NotImplementedException();
            }

            if (municipality.Taxes == null || municipality.Taxes.Count == 0)
            {
                return new MunicipalityWithTax(municipality, 0);
            }

            var frequencies = new[] { TaxFrequency.Daily, TaxFrequency.Weekly, TaxFrequency.Monthly, TaxFrequency.Yearly };

            foreach (var frequency in frequencies)
            {
                var taxes = municipality.Taxes.Where(x => x.Frequency == frequency);

                var tax = Calculate(taxes, date);

                if (tax != null)
                {
                    return new MunicipalityWithTax(municipality, tax.Value);
                }
            }

            return new MunicipalityWithTax(municipality, 0);
        }

        private static Tax Calculate(IEnumerable<Tax> taxes, DateTime date)
        {
            foreach (var tax in taxes)
            {
                if (date.IsBetweenTwoDates(tax.StartDate, tax.EndDate))
                {
                    return tax;
                }
            }

            return null;
        }
    }
}
