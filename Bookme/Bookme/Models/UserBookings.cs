using System.ComponentModel.DataAnnotations.Schema;

namespace Bookme.Models
{
    public class UserBookings
    {
        public int Id { get; set; }
        public string BookedById { get; set; }
        [ForeignKey("BookedById")]
        public virtual ApplicationUser BookedBy { get; set; }

        public string BookedUserId { get; set; }
        [ForeignKey("BookedUserId")]
        public virtual ApplicationUser BookedUser { get; set; }

    }
}
