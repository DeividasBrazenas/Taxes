using System.Collections.Generic;

namespace Taxes.Service.DataLayer.Models
{
    public class Municipality : BaseModel
    {
        public virtual string Name { get; set; }
        public virtual ICollection<Tax> Taxes { get; set; }
    }
}
