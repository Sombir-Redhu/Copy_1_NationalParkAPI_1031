using Copy_1_NationalParkWebAPI_1031.Models;
using Copy_1_NationalParkWebAPI_1031.Models.ViewModels;
using Copy_1_NationalParkWebAPI_1031.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Stripe;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Exceptions;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Copy_1_NationalParkWebAPI_1031.Controllers
{
    public class BookingTripController : Controller
    {
        // Dependency injections for repositories and settings
        private readonly ITrailRepository _trailRepository;
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly IBookingTripRepository _bookTripRepository;
        private readonly IOptions<TwilioSetting> _twilioSetting;
        private readonly MessageSender _messageSender;

        // Constructor for initializing repositories and settings
        public BookingTripController(
            INationalParkRepository nationalParkRepository,
            ITrailRepository trailRepository,
            IBookingTripRepository bookingTripRepository,
            IOptions<TwilioSetting> twilioSetting,
            MessageSender messageSender)
        {
            _nationalParkRepository = nationalParkRepository;
            _trailRepository = trailRepository;
            _bookTripRepository = bookingTripRepository;
            _twilioSetting = twilioSetting;
            _messageSender = messageSender;
        }

        // GET action for booking a trip
        public async Task<IActionResult> BookingTrip(int nationalParkId)
        {
            // Fetch data to populate dropdown lists
            var nationalParks = await _nationalParkRepository.GetAllAsync(SD.NationalParkAPIPath);
            var trails = await _trailRepository.GetAllAsync(SD.TrailAPIPath);

            // Filter trails based on the selected National Park ID
            var selectedNationalParkTrails = trails.Where(t => t.NationalParkId == nationalParkId);

            // Create BookingTripVM and populate lists
            var bookingTripVM = new BookingTripVM
            {
                NationalParkList = new SelectList(nationalParks, nameof(NationalPark.Id), nameof(NationalPark.Name)),
                TrailList = new SelectList(selectedNationalParkTrails, nameof(Trail.Id), nameof(Trail.Name))
            };

            return View(bookingTripVM);
        }

        // POST action for booking a trip
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookingTrip(BookingTripVM bookingTripVM, string stripeToken)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    // Fetch additional data based on user selection (e.g., National Park details)
                    var nationalParkDetails = await _trailRepository.GetAllAsync(SD.NationalParkAPIPath);

                    // Combine user-entered data and automatically generated data
                    var bookingTrip = new BookingTrip
                    {
                        // Assigning user-entered data
                        Name = bookingTripVM.BookingTrip.Name,
                        Email = bookingTripVM.BookingTrip.Email,
                        Address = bookingTripVM.BookingTrip.Address,
                        PhoneNumber = bookingTripVM.BookingTrip.PhoneNumber,
                        TrailId = bookingTripVM.Trail.Id,
                        BookingForDate = bookingTripVM.BookingTrip.BookingForDate,
                        TotalPrice = bookingTripVM.BookingTrip.TotalPrice,
                        Children = bookingTripVM.BookingTrip.Children,
                        Adults = bookingTripVM.BookingTrip.Adults,

                        // Automatically generated data
                        BookedDate = DateTime.Now,
                        PaymentDate = DateTime.Now,
                        BookingStatus = Models.BookingTrip.BookingStatustType.Pending,
                        PaymentStatus = Models.BookingTrip.PaymentStatusType.Pending,
                    };

                    // Handling payment processing with Stripe
                    if (stripeToken == null)
                    {
                        // No Stripe token, mark as pending
                        bookingTrip.BookingStatus = Models.BookingTrip.BookingStatustType.Pending;
                        bookingTrip.PaymentStatus = Models.BookingTrip.PaymentStatusType.Pending;
                    }
                    else
                    {
                        // Stripe token available, process payment
                        var options = new ChargeCreateOptions()
                        {
                            Amount = Convert.ToInt32(bookingTrip.TotalPrice),
                            Currency = "usd",
                            Description = "Order Id : " + bookingTrip.Id,
                            Source = stripeToken
                        };

                        var service = new ChargeService();
                        Charge charge = service.Create(options);

                        if (charge.BalanceTransactionId == null)
                            bookingTrip.PaymentStatus = Models.BookingTrip.PaymentStatusType.Failed;
                        else
                            bookingTrip.TransactionId = charge.BalanceTransactionId;

                        if (charge.Status.ToLower() == "succeeded")
                        {
                            bookingTrip.BookingStatus = Models.BookingTrip.BookingStatustType.Confirmed;
                            bookingTrip.PaymentStatus = Models.BookingTrip.PaymentStatusType.Completed;
                        }
                    }

                    // Saving the booking trip data
                    await _bookTripRepository.CreateAsync(SD.BookingTripAPIPath, bookingTrip);

                    // Sending SMS notification using Twilio

                    try
                    {
                        var emoji = "🏞️"; // Nature emoji
                        var message = MessageResource.Create(
                            body: $"Thank you for booking your trip with us!\n\n" +
                                $"Your Booking Details:\n" +
                                $"- Booking ID: {bookingTrip.Id}\n" +
                                $"- Total Price: {bookingTrip.TotalPrice}\n" +
                                $"- Booking Date: {bookingTrip.BookedDate.ToString("dd MMMM, yyyy")}\n" +
                                $"- Customer Name: {bookingTrip.Name}\n" +
                                $"- Email Address: {bookingTrip.Email}\n" +
                                $"- Address: {bookingTrip.Address}\n" +
                                $"- Trail ID: {bookingTrip.TrailId}\n" +
                                $"- Adults: {bookingTrip.Adults}\n" +
                                $"- Children: {bookingTrip.Children}\n" +
                                $"- Booking Status: {bookingTrip.BookingStatus}\n" +
                                $"- Payment Status: {bookingTrip.PaymentStatus}\n\n" +
                                $"We're looking forward to seeing you in the great outdoors! 🌲",
                            from: new Twilio.Types.PhoneNumber(_twilioSetting.Value.TwilioPhoneNumber),
                            to: new Twilio.Types.PhoneNumber("+91" + bookingTrip.PhoneNumber)
                        );

                    }
                    catch (ApiException ex)
                    {
                        // Log the Twilio error message for debugging
                        Console.WriteLine($"Twilio Error Message: {ex.Message}");

                        // Handle the error as needed
                    }

                    //await _messageSender.SendSmsAsync("+91" + bookingTrip.PhoneNumber, $"Booked Trip Details: {message}");


                    // Redirect to success page or return JSON response
                    // Here you may want to redirect to a success page or return JSON response indicating success.
                }
                catch (Exception )
                {
                    // Log exceptions
                    return Json(new { success = false, message = "Invalid data, unable to book the trip.", errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });

                }
            }

            // Return validation errors if ModelState is not valid
                    return Json(new { success = false, message = "An error occurred while processing your request. Please try again later." });

        }

        // GET action for Index view
        public IActionResult Index()
        {
            // Placeholder for the Index action
            return View();
        }

        // GET action to retrieve all trails
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Retrieve all trails from repository
            return Json(new { data = await _trailRepository.GetAllAsync(SD.TrailAPIPath) });
        }
    }
}
