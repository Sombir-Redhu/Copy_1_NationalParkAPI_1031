using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Copy_1_NationalParkAPI_1031.Models
{
    public class BookingTrip
    {
        public int Id { get; set; }

        //[Required]
        public string Name { get; set; }

        //[Required]
        //[EmailAddress]
        public string Email { get; set; }

        //[Required]
        public string Address { get; set; }

        //[Required]
        public string PhoneNumber { get; set; }

        //[Required]
        public int TrailId { get; set; }

        [ForeignKey("TrailId")]
        public Trail Trail { get; set; }

        public DateTime BookedDate { get; set; }

        //[Required]
        public DateTime BookingForDate { get; set; }

        //[Required]
        public decimal TotalPrice { get; set; }

        public enum BookingStatustType
        {
            Pending,
            Confirmed,
            Cancelled
        }
        public BookingStatustType BookingStatus { get; set; }
        public DateTime? PaymentDate { get; set; }

        public enum PaymentStatusType
        {
            Pending,
            Completed,
            Refund,
            Failed
        }
        public PaymentStatusType PaymentStatus { get; set; }

        //[Required]
        public string TransactionId { get; set; }

        //[Required]
        public int Children { get; set; }

        //[Required]
        public int Adults { get; set; }

        //[NotMapped]
        public int TotalPeople => Children + Adults;
    }
}
