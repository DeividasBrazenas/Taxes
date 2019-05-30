using System.Collections.Generic;

namespace Taxes.Service.DataLayer.Models
{
    public class Municipality : BaseModel
    {
        public string Name { get; set; }
        public ICollection<Tax> Taxes { get; set; }
    }
}
