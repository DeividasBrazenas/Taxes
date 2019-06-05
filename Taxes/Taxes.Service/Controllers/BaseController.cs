using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Taxes.Service.DataLayer.Repositories;
using Taxes.Service.Exceptions;

namespace Taxes.Service.Controllers
{
    public class BaseController<T> : ODataController where T : IEntity
    {
        internal readonly IBaseRepository<T> Repository;

        public BaseController(IBaseRepository<T> repository)
        {
            Repository = repository;
        }

        [HttpGet]
        [EnableQuery]
        public IActionResult Get(int key)
        {
            T entity;

            try
            {
                entity = Repository.FindById(key);
            }
            catch (NotFoundException e)
            {
                return NotFound(e);
            }

            return Ok(entity);
        }

        [HttpGet]
        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(Repository.Get());
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]T entity)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.SelectMany(x => x.Value.Errors, (y, z) => z.Exception.Message);

                return BadRequest(errors);
            }

            T createdEntity;

            try
            {
                createdEntity = await Repository.Add(entity);
            }
            catch (DBConcurrencyException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

            return Created(createdEntity);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]T entity)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .SelectMany(x => x.Value.Errors, (y, z) => z.Exception.Message);

                return BadRequest(errors);
            }

            T updatedEntity;
            try
            {
                updatedEntity = await Repository.Update(entity);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

            return Updated(updatedEntity);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete([FromODataUri] int key)
        {
            try
            {
                await Repository.Delete(key);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

            return NoContent();
        }
    }
}
