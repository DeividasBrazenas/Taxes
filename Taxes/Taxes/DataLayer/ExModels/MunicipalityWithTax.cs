using System;
using Taxes.Service.DataLayer.Models;

namespace Taxes.Service.DataLayer.ExModels
{
    public sealed class MunicipalityWithTax : Municipality
    {
        public MunicipalityWithTax(Municipality municipality, double taxValue)
        {
            this.Name = municipality.Name;
            TaxValue = taxValue;
        }

        public double TaxValue { get; set; }
    }
}
