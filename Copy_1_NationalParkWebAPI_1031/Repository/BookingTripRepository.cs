using Copy_1_NationalParkWebAPI_1031.Models;
using Copy_1_NationalParkWebAPI_1031.Repository.IRepository;

namespace Copy_1_NationalParkWebAPI_1031.Repository
{
    public class BookingTripRepository : Repository<BookingTrip> , IBookingTripRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public BookingTripRepository(IHttpClientFactory httpClientFactory) : base(httpClientFactory)    
        {
            _httpClientFactory = httpClientFactory;
        }
    }
    
}
