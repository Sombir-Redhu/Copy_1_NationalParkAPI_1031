namespace Copy_1_NationalParkWebAPI_1031
{
    public static class SD
    {
        public static string APIBaseUrl = "https://localhost:7002/";
        public static string NationalParkAPIPath = APIBaseUrl + "api/NationalPark";
        public static string TrailAPIPath = APIBaseUrl + "api/Trail";
        public static string BookingTripAPIPath = APIBaseUrl + "api/BookingTrip";

        // Ticket price of child and adult
        public const int ChildrenTicketPrice = 25;
        public const int AdultTicketPrice = 50;
    }
}
