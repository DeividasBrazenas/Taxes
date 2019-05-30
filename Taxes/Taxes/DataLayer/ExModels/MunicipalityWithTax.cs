using Taxes.Service.DataLayer.Models;

namespace Taxes.Service.DataLayer.ExModels
{
    public class MunicipalityWithTax : Municipality
    {
        public double TaxValue { get; set; }
    }
}
