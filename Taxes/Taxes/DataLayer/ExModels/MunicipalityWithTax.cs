using Taxes.Service.DataLayer.Models;

namespace Taxes.Service.DataLayer.ExModels
{
    public class MunicipalityWithTax : Municipality
    {
        public MunicipalityWithTax(Municipality municipality, double taxValue)
        {
            Municipality = municipality;
            TaxValue = taxValue;
        }

        public Municipality Municipality { get; set; }
        public double TaxValue { get; set; }
    }
}
