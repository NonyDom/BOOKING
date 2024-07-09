using System.ComponentModel.DataAnnotations.Schema;
using static Bookme.Database.Enum;

namespace Bookme.Models
{
    public class BookingForm
    {
        public Guid Id { get; set; }
        public string Venue { get; set; }
        public string State { get; set; }
        public string NatureOfEvent { get; set; }
        public string DateOfEvent { get; set; }
        public string? DurationOfEvent { get; set; }
        public string DepartureTime { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime? StatusChangeDate { get; set; }
        public BookingStatus BookingStatus { get; set; }
        public string BookedById { get; set; }
        [ForeignKey("BookedById")]
        public virtual ApplicationUser BookedBy { get; set; }
        public string BookedUserId { get; set; }
        [ForeignKey("BookedUserId")]
        public virtual ApplicationUser BookedUser { get; set; }
    }
}
