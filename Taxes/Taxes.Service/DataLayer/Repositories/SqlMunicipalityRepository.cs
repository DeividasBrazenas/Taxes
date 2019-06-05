using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Taxes.Service.DataLayer.Models;
using Taxes.Service.Exceptions;

namespace Taxes.Service.DataLayer.Repositories
{
    public class SqlMunicipalityRepository : SqlBaseRepository<Municipality>, IMunicipalityRepository
    {
        public SqlMunicipalityRepository(TaxesContext context) : base(context)
        {
        }

        public IEnumerable<Municipality> FindByName(string name)
        {
            var entities = Context.Set<Municipality>().Where(x => x.Name == name).Include(x => x.Taxes);

            if (entities == null)
            {
                throw new NotFoundException("Municipality with the provided name was not found");
            }

            return entities;
        }
    }
}