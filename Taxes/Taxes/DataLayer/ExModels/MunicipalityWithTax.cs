using Taxes.Service.DataLayer.Models;

namespace Taxes.Service.DataLayer.ExModels
{
    public class MunicipalityWithTax : Municipality
    {
        public MunicipalityWithTax(Municipality municipality, double taxValue)
        {
            this.Name = municipality.Name;
            this.Id = municipality.Id;
            TaxValue = taxValue;
        }

        public double TaxValue { get; set; }
    }
}
