using System;
using System.ComponentModel.DataAnnotations;
using Taxes.Service.DataLayer.Repositories;

namespace Taxes.Service.DataLayer.Models
{
    public class Tax : IEntity
    {
        public int Id { get; set; }

        [Required]
        public int MunicipalityId { get; set; }

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
