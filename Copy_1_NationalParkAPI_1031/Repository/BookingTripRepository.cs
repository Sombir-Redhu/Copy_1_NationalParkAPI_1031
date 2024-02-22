using Copy_1_NationalParkAPI_1031.Data;
using Copy_1_NationalParkAPI_1031.Models;
using Copy_1_NationalParkAPI_1031.Repository.IRepository;
using System.Threading.Channels;

namespace Copy_1_NationalParkAPI_1031.Repository
{
    public class BookingTripRepository : IBookingTripRepository
    {
        private readonly ApplicationDbContext _context;
        public BookingTripRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool BookTrip(BookingTrip bookingTrip)
        {
            try
            {
                _context.BookingTrips.Add(bookingTrip);
                return Save();
            }
           catch (Exception ek)
            {
                Console.WriteLine(ek);
                return false;
            }
        }

        public bool CancelTrip(BookingTrip bookingTripId)   //here table is update
        {
            try
            { 
                _context.BookingTrips.Update(bookingTripId);
                return Save();
            }
            catch (Exception ek)
            {
                Console.WriteLine(ek);
                return false;
            }
        }

        public bool Save()
        {
            return _context.SaveChanges() == 1 ? true : false;
        }
    }
}
