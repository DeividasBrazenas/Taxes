using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Taxes.Service.DataLayer.Models
{
    public class Municipality : BaseModel
    {
        [Required]
        public string Name { get; set; }
        public ICollection<Tax> Taxes { get; set; }
    }
}
