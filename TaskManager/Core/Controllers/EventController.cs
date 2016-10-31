namespace TaskManager.Controllers
{
    using Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.OData;

    public class EventController : ODataController
    {
        // EventsContext data = new EventsContext();
        DataStore data = new DataStore();

        private bool EventExists(int id)
        {
            return data.Find(id) != null;
        }
        
        [EnableQuery]
        public IQueryable<Event> Get()
        {
            return data.GetAll();
        }

        [EnableQuery]
        public SingleResult<Event> Get([FromODataUri] int id)
        {
            IQueryable<Event> result = data.Find(id);
            return SingleResult.Create(result);
        }

        public async Task<IHttpActionResult> Post(Event newEvent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await data.PostAsync(newEvent);

            return Created(newEvent);
        }

        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Event> updatedEvent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IQueryable<Event> entity = await data.FindAsync(key);
            if (entity == null || entity.Count() != 1)
            {
                return NotFound();
            }

            updatedEvent.Patch(entity.First());
            try
            {
                await data.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(entity);
        }

        public async Task<IHttpActionResult> Put([FromODataUri] int key, Event update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != update.Id)
            {
                return BadRequest();
            }

            data.Entry(update).State = EntityState.Modified;
            try
            {
                await data.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(update);
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            IQueryable<Event> eventItem = await data.FindAsync(key);
            if (eventItem == null || eventItem.Count() != 1)
            {
                return NotFound();
            }

            data.Remove(eventItem.First());
            await data.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}