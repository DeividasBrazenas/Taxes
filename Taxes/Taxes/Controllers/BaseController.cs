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

        [HttpGet]
        [EnableQuery]
        public IActionResult Get(int key)
        {
            return Ok(Context.Set<T>().FirstOrDefault(c => c.Id == key));
        }

        [HttpGet]
        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(Context.Set<T>());
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]T baseObject)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .SelectMany(x => x.Value.Errors, (y, z) => z.Exception.Message);

                return BadRequest(errors);
            }

            await Context.Set<T>().AddAsync(baseObject);
            await Context.SaveChangesAsync();
            return Created(baseObject);
        }

        [HttpPatch]
        public async Task<IActionResult> Patch([FromODataUri] int key, [FromBody]Delta<T> instance)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .SelectMany(x => x.Value.Errors, (y, z) => z.Exception.Message);

                return BadRequest(errors);
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

        [HttpPut]
        public async Task<IActionResult> Put([FromODataUri]int key, [FromBody]T update)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .SelectMany(x => x.Value.Errors, (y, z) => z.Exception.Message);

                return BadRequest(errors);
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

        [HttpDelete]
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
