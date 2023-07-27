using ReservationApp.Api.DbContext;
using ReservationApp.Api.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;

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
        [Route("getAll")]
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

        [HttpGet]
        [Route("getByName")]
        public async Task<List<Reservation>?> Get(string name)
        {
            var entity = await _set.Where(x => EF.Functions.Like(x.Name, "%" + name + "%")).ToListAsync();
            return entity;
        }

        [HttpGet]
        [Route("getByDateRange")]
        public async Task<List<Reservation>?> Get(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate) return null;
            var entity = await _set.Where(x => x.ReservationDate.Date >= startDate.Date && x.ReservationDate.Date <= endDate.Date).ToListAsync();
            return entity;
        }

        [HttpPost]
        public async Task<string> Add(Reservation request)
        {
            var validation = IsValid(request).Result;
            if (validation.IsSuccess)
            {
                request.CreatedAt = DateTime.Now;
                _set.Add(request);
                await _context.SaveChangesAsync();
                return "Reservation created successfully!";
            }
            else return validation.Message;
        }

        [HttpPut]
        public async Task<string> Update(Reservation request)
        {
            var validation = IsValid(request).Result;
            if (validation.IsSuccess)
            {
                request.UpdatedAt = DateTime.Now;
                _set.Update(request);
                await _context.SaveChangesAsync();
                return "Reservation updated successfully!";
            }
            else return validation.Message;
        }

        [HttpDelete]
        public async Task<string> Delete(int id)
        {
            var entity = await Get(id);
            if (entity != null)
            {
                _set.Remove(entity);
                await _context.SaveChangesAsync();
                return "Reservation deleted successfully!";
            }
            return "No reservation found!";
        }

        private async Task<Response> IsValid(Reservation request)
        {
            var id = request.Id;
            var startHour = request.StartHour;
            var endHour = request.StartHour + request.Duration;
            var date = request.ReservationDate;
            var dayOfWeek = date.DayOfWeek.ToString();
            var duration = request.Duration;

            // Out of working hours
            if (startHour < 9 || endHour > 17 || dayOfWeek == "Saturday" || dayOfWeek == "Sunday")
                return new Response(false, "Date range is out of working hours!");

            // Max 4 - Min 1 hours duration
            if (duration < 1 || duration > 4)
                return new Response(false, "Duration should be between 1 and 4 hours!");

            // Already booked
            var entity = await _set.SingleOrDefaultAsync(x => x.Id != id && x.ReservationDate.Date == date.Date && // Diff Record && Same Day
                        ((x.StartHour >= startHour && x.StartHour < endHour) ||  // Start hour of the record btw request start and end hour
                        ((x.StartHour + x.Duration) > startHour && (x.StartHour + x.Duration) <= endHour))); // End hour of the record btw request start and end hour

            if (entity != null) return new Response(false, "This date range is already booked!");

            // Valid
            return new Response(true, "Success");
        }
    }
}
