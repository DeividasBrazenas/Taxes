using System;
using System.ComponentModel.DataAnnotations;

namespace Taxes.Service.DataLayer.Models
{
    public class Tax : BaseModel
    {
        [Required]
        public int MunicipalityId { get; set; }

        [Required]
        public Municipality Municipality { get; set; }

        [Required]
        public TaxFrequency Frequency { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
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
