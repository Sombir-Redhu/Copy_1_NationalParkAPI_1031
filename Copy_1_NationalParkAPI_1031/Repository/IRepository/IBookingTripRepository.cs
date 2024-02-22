using Copy_1_NationalParkAPI_1031.Models;

namespace Copy_1_NationalParkAPI_1031.Repository.IRepository
{
    public interface IBookingTripRepository
    {
        bool BookTrip(BookingTrip bookingTrip);
        bool CancelTrip(BookingTrip bookingTripId);
        bool Save();
    }
}
