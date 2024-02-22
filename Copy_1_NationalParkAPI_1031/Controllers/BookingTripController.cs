using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Copy_1_NationalParkAPI_1031.Models;
using Copy_1_NationalParkAPI_1031.Repository.IRepository;
using Copy_1_NationalParkAPI_1031.Data;

namespace Copy_1_NationalParkAPI_1031.Controllers
{
    [Route("api/BookingTrip")]
    [ApiController]
    public class BookingTripController : ControllerBase
    {
        private readonly ApplicationDbContext _context; // Assuming ApplicationDbContext is your DbContext
        private readonly IBookingTripRepository _bookTripRepository;
        public BookingTripController(ApplicationDbContext context, IBookingTripRepository bookingTripRepository)
        {
            _context = context;
            _bookTripRepository = bookingTripRepository;
        }

        [HttpPost]
        public IActionResult BookTrip(BookingTrip bookingTrip)
        {
            if (!ModelState.IsValid) BadRequest();
            if (!_bookTripRepository.BookTrip(bookingTrip))
            {
                ModelState.AddModelError("", $"Somthing went wrong while Update data : {bookingTrip.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }

        // PUT: api/BookingTrip/CancelTrip/5
        [HttpPut]
        public IActionResult CancelTrip(BookingTrip bookingTripId)
        {
            var bookingTrip = _context.BookingTrips.FirstOrDefault(bookingTripId);

            if (bookingTrip == null)
            {
                return NotFound($"Trip with Id {bookingTripId} not found");
            }

            try
            {
                bookingTrip.BookingStatus= BookingTrip.BookingStatustType.Cancelled;
                _bookTripRepository.CancelTrip(bookingTrip);
                return Ok($"Trip with Id {bookingTripId} canceled successfully");
            }
            catch (Exception)
            {
                return StatusCode(500, $"Error while canceling trip with Id {bookingTripId}");
            }
        }
    }
}
