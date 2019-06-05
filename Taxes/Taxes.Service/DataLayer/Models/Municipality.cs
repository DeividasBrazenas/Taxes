using System.Collections.Generic;
using Taxes.Service.DataLayer.Repositories;

namespace Taxes.Service.DataLayer.Models
{
    public class Municipality : IEntity
    {
        public int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual ICollection<Tax> Taxes { get; set; }
    }
}
