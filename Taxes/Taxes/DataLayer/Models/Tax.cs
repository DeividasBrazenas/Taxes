using System;

namespace Taxes.Service.DataLayer.Models
{
    public class Tax : BaseModel
    {
        public int MunicipalityId { get; set; }
        public TaxFrequency Frequency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Value { get; set; }
    }

    public enum TaxFrequency
    {
        Daily = 1,
        Weekly = 2,
        Monthly = 3,
        Yearly = 4
    }
}
