using System.ComponentModel;

namespace Bookme.Database
{
    public class Enum
    {
        public enum BookingStatus
        {
            //[Description("For Pending")]
            Pending = 1,
            Approved = 2,
            Declined =3,
            Cancel = 4
        }
    }
}
