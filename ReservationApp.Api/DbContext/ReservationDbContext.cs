using EF = Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Api.Entity;

namespace ReservationApp.Api.DbContext
{
    public class ReservationDbContext : EF.DbContext
    {
        public DbSet<Reservation> Reservations { get; set; }
        public ReservationDbContext(DbContextOptions options) : base(options) { }
    }
}