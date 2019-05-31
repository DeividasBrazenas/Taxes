using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Taxes.Service.DataLayer;
using Taxes.Service.DataLayer.Models;

namespace Taxes.Service.Controllers
{
    public class BaseController<T> : ODataController where T : BaseModel
    {
        protected readonly TaxesContext Context;

        public BaseController(TaxesContext context)
        {
            Context = context;
        }

        [EnableQuery]
        public IActionResult Get(int key)
        {
            return Ok(Context.Set<T>().FirstOrDefault(c => c.Id == key));
        }

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(Context.Set<T>());
        }

        [EnableQuery]
        public IActionResult Post([FromBody]T baseObject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Context.Set<T>().Add(baseObject);
            Context.SaveChanges();
            return Created(baseObject);
        }

        public async Task<IActionResult> Patch([FromODataUri] int key, [FromBody] Delta<T> instance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await Context.Set<T>().FindAsync(key);

            if (entity == null)
            {
                return NotFound();
            }

            instance.Patch(entity);

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModelExists(key))
                {
                    return NotFound();
                }

                throw new NotImplementedException();
            }

            return Updated(entity);
        }

        public async Task<IActionResult> Put([FromODataUri]int key, [FromBody] T update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != update.Id)
            {
                return BadRequest();
            }

            Context.Entry(update).State = EntityState.Modified;

            try
            {
                await Context.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException)
            {
                if (!ModelExists(key))
                {
                    return NotFound();
                }

                throw new NotImplementedException();
            }

            return Updated(update);
        }

        public async Task<ActionResult> Delete([FromODataUri] int key)
        {
            var movie = await Context.Set<T>().FindAsync(key);

            if (movie == null)
            {
                return NotFound();
            }

            Context.Set<T>().Remove(movie);
            await Context.SaveChangesAsync();

            return StatusCode((int)System.Net.HttpStatusCode.NoContent);
        }

        private bool ModelExists(int key)
        {
            return Context.Set<T>().Any(x => x.Id == key);
        }
    }
}
