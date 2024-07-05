using Bookme.Models;
using static Bookme.Database.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookme.ViewModels
{
    public class BookingFormViewModel
    {
        public Guid Id { get; set; }
        public string Venue { get; set; }
        public string State { get; set; }
        public string NatureOfEvent { get; set; }
        public string DateOfEvent { get; set; }
        public string DurationOfEvent { get; set; }
        public string DepartureTime { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime? StatusChangeDate { get; set; }
        public BookingStatus BookingStatus { get; set; }
        public string BookedById { get; set; }
        public virtual ApplicationUser BookedBy { get; set; }
        public string BookedUserId { get; set; }
        public string BookedUserName { get; set; }
        public string LogedInUser { get; set; }
        public int TotalBooking {  get; set; }
        public int TotalApprovedBooking { get; set; }
        public int TotalDeclinedBooking {  get; set; }
        public int TotalCancelledBooking { get; set; }
        public int TotalPendingBooking { get; set; }
    }
}   
