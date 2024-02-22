using Microsoft.EntityFrameworkCore;
using Copy_1_NationalParkAPI_1031.Models;

namespace Copy_1_NationalParkAPI_1031.Data
{
    public class ApplicationDbContext : DbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { 
        }
        public DbSet<NationalPark> NationalParks { get; set; }
        public DbSet<Trail> Trails { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BookingTrip> BookingTrips { get; set; }    
    }
}
