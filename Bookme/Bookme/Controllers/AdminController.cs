using Bookme.Database;
using Bookme.Helper;
using Bookme.IHelper;
using Bookme.Models;
using Bookme.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace Bookme.Controllers
{
   
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAdminHelper _adminHelper;
        private readonly IUserHelper _userHelper;
        public AdminController(ApplicationDbContext context, IAdminHelper adminHelper, IUserHelper userHelper)
        {
            _context = context;
            _adminHelper = adminHelper;
            _userHelper = userHelper;
        }
        public IActionResult Index()
        {
            var loggedInUser = _adminHelper.GetLoggedInUser(User.Identity.Name);
            if (loggedInUser != null) 
            {
                var getTotalBooking = _adminHelper.GetTotalBooking(loggedInUser.Id);
                var GetTotalApprovedBooking = _adminHelper.GetTotalApprovedBooking(loggedInUser.Id);
                var GetTotalDeclinedBooking =_adminHelper.GetTotalDeclinedBooking(loggedInUser.Id);
                var GetTotalCancelledBooking = _adminHelper.GetTotalCancelledBooking(loggedInUser.Id);
                var GetTotalPendingBooking = _adminHelper.GetTotalPendingBooking(loggedInUser.Id);

                var model = new BookingFormViewModel 
                { 
                    TotalBooking = getTotalBooking,
                    TotalApprovedBooking = GetTotalApprovedBooking,
                    TotalDeclinedBooking = GetTotalDeclinedBooking,
                    TotalCancelledBooking = GetTotalCancelledBooking,
                    TotalPendingBooking = GetTotalPendingBooking,
                    UserId = loggedInUser.Id,

                };
                return View(model);
            }
            return View();
        }

        public IActionResult MyBookings()
        {
            var loggedInUser = _adminHelper.GetLoggedInUser(User?.Identity?.Name);
            if (loggedInUser != null)
            {
                var bookings = _adminHelper.MyBookingList(loggedInUser.Id);
                return View(bookings);
            }
            return View();
        }

        
        public IActionResult MyBookingHistory() 
        {
            var loggedInUser = _adminHelper.GetLoggedInUser(User?.Identity?.Name);
            if (loggedInUser != null)
            {
                var bookingHistory = _adminHelper.MyBookingHistory(loggedInUser.Id);
                return View(bookingHistory);
            }
            return View();
        }

        public IActionResult Profile()
        {
            var userId = _userHelper.GetCurrentUserId(User.Identity.Name);
            var user = _userHelper.FindById(userId);
            var profile = new ApplicationUserViewModel
            {
                User = user,
                Category = user.Category,
            };
            return View(profile);
        }

        public IActionResult Booking()
        {
            var loggedInUser = _adminHelper.GetLoggedInUser(User?.Identity?.Name);
            if (loggedInUser != null)
            {
                var listOfKeyboardist = _adminHelper.GroupUsersByCategory(loggedInUser.Id);
                return View(listOfKeyboardist);
            }
            return View();
        }

        [HttpGet]
        public IActionResult BookingForm(string id) 
        {
            if (!string.IsNullOrEmpty(id)) 
            {
                var getbookedUserId = _context.ApplicationUser.Where(x => x.Id == id && !x.IsDeactivated && x.IsAvailable == true).FirstOrDefault();
                var model = new BookingFormViewModel
                {
                    BookedUserId = getbookedUserId?.Id,
                };
                return View(model);
            }
            return View();
        }

        [HttpPost]
        public IActionResult CreateBookingForm(BookingForm bookingForm)
        {
            if (bookingForm != null)
            {
                var loggedInUser = _adminHelper.GetLoggedInUser(User?.Identity?.Name);
                if (loggedInUser != null) 
                {
                    var show = new BookingForm
                    {
                        DateOfEvent = bookingForm.DateOfEvent,
                        Venue = bookingForm.Venue,
                        State = bookingForm.State,
                        NatureOfEvent = bookingForm.NatureOfEvent,
                        DurationOfEvent = bookingForm.DurationOfEvent,
                        DepartureTime = bookingForm.DepartureTime,
                        BookingDate = DateTime.Now,
                        StatusChangeDate = bookingForm.StatusChangeDate,
                        BookingStatus = Database.Enum.BookingStatus.Pending,
                        BookedUserId = bookingForm.BookedUserId,
                        BookedById = loggedInUser.Id,
                    };
                    _context.BookingForm.Add(show);
                    _context.SaveChanges();
                    TempData["Message"] = "";
                    return RedirectToAction("Booking");
                }
            }
            return View();
        }



        public IActionResult ViewMore(int categoryId)
        {
            if (categoryId > 0)
            {
                var loggedInUser = _adminHelper.GetLoggedInUser(User?.Identity?.Name);
                if (loggedInUser != null)
                {
                    var listOfUsers = _adminHelper.ViewAllUsersInCategory(categoryId,loggedInUser.Id);
                    return View(listOfUsers);
                }
            }
            return View();
        }

        public JsonResult ApprovedBookings(Guid id)
        {
            if (id != Guid.Empty)
            {
                var checkIfApproved = _adminHelper.CheckForApprovedBooking(id);
                if (checkIfApproved)
                {
                    return Json(new { isError = true, msg = "This booking has been approved before" });
                }
                var approve = _adminHelper.ApproveMyBookings(id);
                if (approve)
                {
                    return Json(new { isError = false, msg = "Booking Approved successfully" });
                }
                return Json(new { isError = true, msg = "bbbbb" });
            }
            return Json(new { isError = true, msg = " There is no Booking to Approve" });
        }

        public JsonResult DeclineBookings(Guid id)
        {
            if (id != Guid.Empty)
            {
                var checkIfDeclined = _adminHelper.CheckForDeclinedBooking(id);
                if (checkIfDeclined)
                {
                    return Json(new { isError = true, msg = "This booking has been declined before" });
                }
                var decline = _adminHelper.DeclineMyBookings(id);
                if (decline)
                {
                    return Json(new { isError = false, msg = "Booking Declined" });
                }
                return Json(new { isError = true, msg = "bbbbb" });
            }
            return Json(new { isError = true, msg = " There is no Booking to Decline" });
        }

        public JsonResult CancelBookings(Guid id)
        {
            if (id != Guid.Empty)
            {
                var checkIfCancelled = _adminHelper.CheckForCancelledBooking(id);
                if (checkIfCancelled)
                {
                    return Json(new { isError = true, msg = "This booking has been cancelled before" });
                }
                var decline = _adminHelper.CancelMyBookings(id);
                if (decline)
                {
                    return Json(new { isError = false, msg = "Booking Cancelled" });
                }
                return Json(new { isError = true, msg = "bbbbb" });
            }
            return Json(new { isError = true, msg = " There is no Booking to Cancel" });
        }

        [HttpGet]
        public JsonResult GetUserProfile(string userId)
        {
            if (userId != null)
            {
                var user = _adminHelper.GetProfileById(userId);
                if (user != null)
                {
                    return Json(new { isError = false, data = user });
                }
            }
            return Json(new { isError = true, msg = "Couldn't Edit" });
        }

        [HttpPost]
        public JsonResult UpdateUserProfile(string userDetails)
        {
            if (userDetails == null)
            {
                return Json(new { isError = true, msg = "No User Found" });
            }
            var data = JsonConvert.DeserializeObject<ApplicationUserViewModel>(userDetails);
            {
                if (data != null)
                {
                    var updatedProfile = _adminHelper.UpdateUserProfile(data);
                    if (updatedProfile)
                    {
                        return Json(new { isError = false, msg = "Profile updated successfully", updatedProfile });
                    }
                        return Json(new { isError = true, msg = "Profile Couldn't Update" });

                }
            }
            return Json(new { isError = true, msg = "Error occured" });
        }

        [HttpGet]
        public JsonResult getProfilePicture(string userId)
        {
            if (userId != null)
            {
                var user = _adminHelper.GetProfileById(userId);
                if (user != null)
                {
                    return Json(new { isError = false, data = user });
                }
            }
            return Json(new { isError = true, msg = "Couldn't Edit" });
        }

        [HttpPost]
        public JsonResult UpdatePicture(string base64, string id)
        {
            if (base64 == null && id == null )
            {
                return Json(new { isError = true, msg = "No picture Found" });
            }
             else
                {
                    var UpdatePicture = _adminHelper.UpdateProfilePicture(base64, id);
                    if (UpdatePicture)
                    {
                        return Json(new { isError = false, msg = "Profile picture updated successfully",});
                    }
                    return Json(new { isError = true, msg = "Profile picture Couldn't Update" });

                }
            
            return Json(new { isError = true, msg = "Error occured" });
        }

        [HttpGet]
        public IActionResult ViewDetails(string id)
        {
            if (id != null)
            {
                var user = _adminHelper.GetProfileById(id);
                if (user != null)
                {
                    var hhh = new ApplicationUserViewModel
                    {
                        User= user,
                    };
                    return PartialView(hhh);
                }
            }
            return View();
            
        }

        [HttpPost]
        public JsonResult Available(string userId)
        {
            if (userId != null)
            {
                var checkIfAvailable = _adminHelper.changeAvailability(userId);
                if (checkIfAvailable)
                {
                    return Json(new { isError = false, msg = "Availability Changed successfully" });
                }
            }
            return Json(new { isError = true, msg = " You are still available for booking" });
        }

        [HttpPost]
        public JsonResult EditProfile(string userId, string base64)
        {
            if (string.IsNullOrEmpty(base64) && userId == null)
            {
                return Json(new { isError = true, msg = "Error occurred" });
            }
            var uploadUserPicture = _adminHelper.EditProfile(base64, userId);
            return uploadUserPicture
                == null ? Json(new { isError = true, msg = "Unable to edit profile" })
                : Json(new { isError = false, msg = "Profile picture edited successfully" });
        }
    }
}

