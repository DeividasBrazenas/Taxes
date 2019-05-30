using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Taxes.Service.DataLayer.Models
{
    public class MunicipalityWithTax : Municipality
    {
        public double TaxValue { get; set; }
    }
}
