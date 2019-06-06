using System;
using System.ComponentModel.DataAnnotations;
using Taxes.Service.DataLayer.Repositories;

namespace Taxes.Service.DataLayer.Models
{
    public class Tax : BaseModel
    {
        [Required]
        public int MunicipalityId { get; set; }

        public Municipality Municipality { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public double Value { get; set; }
    }
}
