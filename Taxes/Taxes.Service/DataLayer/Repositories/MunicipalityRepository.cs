using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Taxes.Service.DataLayer.Models;
using Taxes.Service.Exceptions;
using Taxes.Service.Extensions;

namespace Taxes.Service.DataLayer.Repositories
{
    public class MunicipalityRepository : BaseRepository<Municipality>, IMunicipalityRepository
    {
        public MunicipalityRepository(TaxesContext context) : base(context)
        {
        }

        public IEnumerable<Municipality> GetMunicipalitiesWithTaxes(string name, DateTime date)
        {
            var entities = Context.Set<Municipality>().Where(x => x.Name == name).Include(x => x.Taxes);

            foreach (var entity in entities)
            {
                entity.Taxes = entity.Taxes.Where(x => date.IsBetweenTwoDates(x.StartDate, x.EndDate)).ToList();
            }

            if (entities == null)
            {
                throw new NotFoundException("No municipalities with the provided name was not found");
            }

            return entities;
        }
    }
}