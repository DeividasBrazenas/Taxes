using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Taxes.Service.DataLayer.Models;
using Taxes.Service.Exceptions;

namespace Taxes.Service.DataLayer.Repositories
{
    public class SqlBaseRepository<T> : IBaseRepository<T> where T : IEntity
    { 
        internal readonly TaxesContext Context;

        public SqlBaseRepository(TaxesContext context)
        {
            Context = context;
        }

        public IEnumerable<T> Get()
        {
            return Context.Set<T>();
        }

        public T FindById(int id)
        {
            var entity = Context.Set<T>().Find(id);

            if (entity == null)
            {
                throw new NotFoundException(typeof(T) + " with the provided id was not found");
            }

            return entity;
        }

        public async Task<T> Add(T entity)
        {
            var createdEntity = await Context.Set<T>().AddAsync(entity);

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DBConcurrencyException ex)
            {
                throw new DBConcurrencyException(ex.Message);
            }

            return createdEntity.Entity;
        }

        public async Task<int> Delete(int id)
        {
            var entity = await Context.Set<T>().FindAsync(id);

            if (entity == null)
            {
                throw new NotFoundException(typeof(T) + " with the provided id was not found");
            }

            Context.Set<T>().Remove(entity);

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DBConcurrencyException ex)
            {
                throw new DBConcurrencyException(ex.Message);
            }

            return id;
        }

        public async Task<T> Update(T entity)
        {
            if (!ModelExists(entity.Id))
            {
                throw new NotFoundException(typeof(T) + " with the provided id was not found");
            }

            Context.Entry(entity).State = EntityState.Modified;

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DBConcurrencyException ex)
            {
                throw new DBConcurrencyException(ex.Message);
            }

            return entity;
        }

        private bool ModelExists(int id)
        {
            return Context.Municipalities.Any(x => x.Id == id);
        }
    }
}