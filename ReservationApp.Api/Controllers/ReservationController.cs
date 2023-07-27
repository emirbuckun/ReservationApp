using ReservationApp.Api.DbContext;
using ReservationApp.Api.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ReservationApp.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationController : ControllerBase
    {
        protected readonly ReservationDbContext _context;
        protected readonly DbSet<Reservation> _set;

        public ReservationController(ReservationDbContext context)
        {
            _context = context;
            _set = _context.Set<Reservation>();
        }


        [HttpGet]
        [Route("list")]
        public async Task<List<Reservation>> Get()
        {
            return await _set.ToListAsync();
        }

        [HttpGet]
        [Route("getById")]
        public async Task<Reservation?> Get(int id)
        {
            var entity = await _set.SingleOrDefaultAsync(x => x.Id == id);
            return entity;
        }

        [HttpPost]
        public async Task<Reservation> Add(Reservation request)
        {
            request.CreatedAt = DateTime.Now;
            _set.Add(request);
            await _context.SaveChangesAsync();
            return request;
        }

        [HttpPut]
        public async Task<Reservation> Update(Reservation request)
        {
            request.UpdatedAt = DateTime.Now;
            _set.Update(request);
            await _context.SaveChangesAsync();
            return request;

        }

        [HttpDelete]
        public async Task<bool> Delete(int id)
        {
            var entity = await Get(id);
            if (entity != null)
            {
                _set.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
