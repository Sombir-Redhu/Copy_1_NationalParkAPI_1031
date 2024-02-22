using Microsoft.AspNetCore.Mvc.Rendering;

namespace Copy_1_NationalParkWebAPI_1031.Models.ViewModels
{
    public class BookingTripVM
    {

        public BookingTrip BookingTrip { get; set; }
        public Trail Trail { get; set; }
        public NationalPark NationalPark { get; set; }
        public IEnumerable<SelectListItem> NationalParkList { get; set; }
        public IEnumerable<SelectListItem> TrailList { get; set; }
    }

}
