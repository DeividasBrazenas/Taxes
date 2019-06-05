using System.Collections.Generic;
using Taxes.Service.DataLayer.Models;

namespace Taxes.Service.DataLayer.Repositories
{
    public interface IMunicipalityRepository : IBaseRepository<Municipality>
    {
        IEnumerable<Municipality> FindByName(string name);
    }
}