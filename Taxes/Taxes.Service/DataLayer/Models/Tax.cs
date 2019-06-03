using System;
using System.ComponentModel.DataAnnotations;

namespace Taxes.Service.DataLayer.Models
{
    public class Tax : BaseModel
    {
        [Required]
        public int MunicipalityId { get; set; }

        [Required]
        public virtual Municipality Municipality { get; set; }

        [Required]
        public virtual TaxFrequency Frequency { get; set; }

        [Required]
        public virtual DateTime StartDate { get; set; }

        [Required]
        public virtual DateTime EndDate { get; set; }

        [Required]
        public virtual double Value { get; set; }
    }

    public enum TaxFrequency
    {
        Daily = 1,
        Weekly = 2,
        Monthly = 3,
        Yearly = 4
    }
}
