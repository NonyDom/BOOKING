using Bookme.Models;
using Bookme.ViewModels;

namespace Bookme.IHelper
{
    public interface IAdminHelper
    {
        bool ApproveMyBookings(Guid id);
        bool CancelMyBookings(Guid id);
        bool changeAvailability(string userId);
        bool CheckForApprovedBooking(Guid id);
        bool CheckForCancelledBooking(Guid id);
        bool CheckForDeclinedBooking(Guid id);
        bool DeclineMyBookings(Guid id);
        Category GetCategoryById(int id);
        ApplicationUser GetLoggedInUser(string username);
        int GetMyPendingBooking(string loggedInUser);
        ApplicationUser GetProfileById(string userId);
        int GetTotalApprovedBooking(string UserId);
        int GetTotalBooking(string UserId);
        int GetTotalCancelledBooking(string UserId);
        int GetTotalCategory();
        int GetTotalDeclinedBooking(string UserId);
        string EditProfile(string base64, string userId);
        int GetTotalPendingBooking(string UserId);
        List<ApplicationUserViewModel> GroupUsersByCategory(string loggedInUserId);
        List<BookingFormViewModel> MyBookingHistory(string loggedInUserId);
        List<BookingFormViewModel> MyBookingList(string logedInUserId);
        bool UpdateCategoryName(CategoryViewModel data);
        bool UpdateProfilePicture(string base64,string id);
        bool UpdateUserProfile(ApplicationUserViewModel data);
        List<ApplicationUserViewModel> ViewAllUsersInCategory(int categoryId, string loggedInUserId);
    }
}
