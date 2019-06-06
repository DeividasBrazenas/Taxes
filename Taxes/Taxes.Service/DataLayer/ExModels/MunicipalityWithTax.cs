using System;
using Taxes.Service.DataLayer.Models;

namespace Taxes.Service.DataLayer.ExModels
{
    public sealed class MunicipalityWithTax : Municipality
    {
        public MunicipalityWithTax(Municipality municipality, Tax tax)
        {
            this.Name = municipality.Name;
            this.Id = municipality.Id;
            TaxValue = tax.Value;
        }

        public double TaxValue { get; set; }
    }
}
