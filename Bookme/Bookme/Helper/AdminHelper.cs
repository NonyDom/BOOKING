using Bookme.Database;
using Bookme.IHelper;
using Bookme.Models;
using Bookme.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Drawing.Text;
using System.Security.Principal;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Bookme.Helper
{
    public class AdminHelper : IAdminHelper
    {
        private readonly ApplicationDbContext _context;
        public AdminHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool CheckEmail(string email)
        {
            var check = _context.ApplicationUser.Where(g => g.Email == email).FirstOrDefault();
            if (check != null) 
            { 
              return true;
            }
            return false;
        }

        public Category GetCategoryById(int id)
        {
            var category = _context.Category.FirstOrDefault(x  => x.Id == id && x.Active && !x.Deleted);
            if (category != null)
            {
                return category;
            }
            return null;
        }
        public Category GetCategoryByName(string name)
        {
            var category = _context.Category.FirstOrDefault(x => x.Name == name && x.Active && !x.Deleted);
            if (category != null)
            {
                return category;
            }
            return null;
        }

        public bool UpdateCategoryName(CategoryViewModel data)
        {
            var checkCategory = _context.Category.Where(x => x.Id == data.Id && x.Active && !x.Deleted).FirstOrDefault();
            if (checkCategory != null)
            {
                checkCategory.Name = data.Name; 
                checkCategory.DateCreated = DateTime.Now;
                _context.Update(checkCategory);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<ApplicationUserViewModel> GroupUsersByCategory(string loggedInUserId)
        {
            try
            {
                if (loggedInUserId != null)
                {
                    var userDetails = _context.ApplicationUser.Where(x => x.Id != loggedInUserId && !x.IsDeactivated && x.CategoryId != null)
                    .Include(b => b.Category).Select(g =>
                    
                        new ApplicationUserViewModel
                        {
                            Image = g.Image,
                            CategoryId = g.CategoryId,
                            CategoryName = g.Category.Name,
                            FirstName = g.FirstName,
                            LastName = g.LastName,
                            UserName = g.UserName,
                            MusicSpecialization = g.MusicSpecialization,
                            Price = g.Price,
                            Id = g.Id,
                            IsAvailable = g.IsAvailable,
                            IsDeactivated = g.IsDeactivated,
                        });
                    
                    
                    var result = userDetails.ToList();
                    return result;
                }
                return null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ApplicationUser GetLoggedInUser(string username)
        {
            if (username != null)
            {
                 return _context.ApplicationUser.Where(a => a.UserName == username && !a.IsDeactivated).FirstOrDefault(); 
            }
            return null;
        }
        public List<BookingFormViewModel> MyBookingList(string loggedInUserId)
        {
            try
            {
                if (!string.IsNullOrEmpty(loggedInUserId))
                {
                    var myBookingList = _context.BookingForm
                    .Where(x => x.BookedUserId == loggedInUserId && (x.BookingStatus == Database.Enum.BookingStatus.Pending || x.BookingStatus == Database.Enum.BookingStatus.Approved))
                    .Select(b => new BookingFormViewModel
                    {
                        Venue = b.Venue,
                        State = b.State,
                        NatureOfEvent = b.NatureOfEvent,
                        DateOfEvent = b.DateOfEvent,
                        DepartureTime = b.DepartureTime,
                        DurationOfEvent = b.DurationOfEvent,
                        BookedBy = b.BookedBy,
                        BookingDate = b.BookingDate,
                        Id = b.Id,
                        BookingStatus = b.BookingStatus,
                    })
                    .ToList();

                    return myBookingList;
                }
                return new List<BookingFormViewModel>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception details here
                throw;
            }
        }

        public List<BookingFormViewModel> MyBookingHistory(string loggedInUserId)
        {
            try
            {
                if (!string.IsNullOrEmpty(loggedInUserId))
                {
                    var myBookingList = _context.BookingForm
                    .Where(x => x.BookedUserId == loggedInUserId || x.BookedById == loggedInUserId)
                    .Select(b => new BookingFormViewModel
                    {
                        Venue = b.Venue,
                        State = b.State,
                        NatureOfEvent = b.NatureOfEvent,
                        DateOfEvent = b.DateOfEvent,
                        DepartureTime = b.DepartureTime,
                        DurationOfEvent = b.DurationOfEvent,
                        BookedBy = b.BookedBy,
                        BookingDate = b.BookingDate,
                        BookingStatus = b.BookingStatus,
                        BookedUserId = b.BookedUserId,
                    })
                    .ToList();

                    //var ggg = _context.ApplicationUser.Where

                    return myBookingList;
                }
                return new List<BookingFormViewModel>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception details here
                throw;
            }
        }

        public bool CheckForApprovedBooking(Guid id) 
        {
            var checkMyBookings = _context.BookingForm.Where(x  => x.Id == id && x.BookingStatus == Database.Enum.BookingStatus.Approved)
                .FirstOrDefault();
            if (checkMyBookings != null)
            {
                return true;
            }
            return false;
        }
        public bool ApproveMyBookings(Guid id)
        {
            var approveMyBookings = _context.BookingForm.Where(x => x.Id == id && x.BookingStatus == Database.Enum.BookingStatus.Pending)
                .FirstOrDefault();
            if (approveMyBookings != null)
            {
                approveMyBookings.BookingStatus = Database.Enum.BookingStatus.Approved;
                approveMyBookings.StatusChangeDate = DateTime.Now;
                _context.Update(approveMyBookings);

                //add availabilitychange when the eventdate is close
                string dateString = approveMyBookings.DateOfEvent; // Assuming this is a string
                DateTime eventDateToConvert = DateTime.Parse(dateString);

                //if (eventDateToConvert > DateTime.Now.AddDays(2))
                //{
                    var updateAvailaibility = _context.ApplicationUser.Where(x => x.Id == approveMyBookings.BookedUserId && !x.IsDeactivated)
                    .FirstOrDefault();
                    if (updateAvailaibility != null)
                    {
                        updateAvailaibility.IsAvailable = false;
                        _context.Update(updateAvailaibility);
                    }
                //}

                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool CheckForDeclinedBooking(Guid id)
        {
            var checkMyBookings = _context.BookingForm.Where(x => x.Id == id && x.BookingStatus == Database.Enum.BookingStatus.Declined)
                .FirstOrDefault();
            if (checkMyBookings != null)
            {
                return true;
            }
            return false;
        }

        public bool DeclineMyBookings(Guid id)
        {
            var DeclineMyBookings = _context.BookingForm.Where(x => x.Id == id && x.BookingStatus == Database.Enum.BookingStatus.Pending)
                .FirstOrDefault();
            if (DeclineMyBookings != null)
            {
                DeclineMyBookings.BookingStatus = Database.Enum.BookingStatus.Declined;
                DeclineMyBookings.StatusChangeDate = DateTime.Now;
                _context.Update(DeclineMyBookings);

                var updateAvailaibility = _context.ApplicationUser.Where(x => x.Id == DeclineMyBookings.BookedUserId && !x.IsDeactivated)
                    .FirstOrDefault();
                if (updateAvailaibility != null)
                {
                    updateAvailaibility.IsAvailable = true;
                    _context.Update(updateAvailaibility);
                }

                _context.SaveChanges();
                return true;
            }
            return false;
        }


        public bool CheckForCancelledBooking(Guid id)
        {
            var checkMyBookings = _context.BookingForm.Where(x => x.Id == id && x.BookingStatus == Database.Enum.BookingStatus.Declined)
                .FirstOrDefault();
            if (checkMyBookings != null)
            {
                return true;
            }
            return false;
        }

        public bool CancelMyBookings(Guid id)
        {
            var CancelMyBookings = _context.BookingForm.Where(x => x.Id == id && x.BookingStatus == Database.Enum.BookingStatus.Pending || x.BookingStatus == Database.Enum.BookingStatus.Approved)
                .FirstOrDefault();
            if (CancelMyBookings != null)
            {
                CancelMyBookings.BookingStatus = Database.Enum.BookingStatus.Cancel;
                CancelMyBookings.StatusChangeDate = DateTime.Now;
                _context.Update(CancelMyBookings);

                var updateAvailaibility = _context.ApplicationUser.Where(x => x.Id == CancelMyBookings.BookedUserId && !x.IsDeactivated)
                    .FirstOrDefault();
                if (updateAvailaibility != null)
                {
                    updateAvailaibility.IsAvailable = true;
                    _context.Update(updateAvailaibility);
                }
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public ApplicationUser GetProfileById(string userId)
        {
            var profile = _context.ApplicationUser.Where(x => x.Id == userId && !x.IsDeactivated)
                .Include(u => u.Category).FirstOrDefault();
            if (profile != null)
            {
                return profile;
            }
            return null;
        }
        public bool UpdateUserProfile(ApplicationUserViewModel data)
        {
            var categoryId = GetCategoryByName(data.CategoryName);

            var checkProfile = _context.ApplicationUser.Where(x => x.Id == data.Id && !x.IsDeactivated).FirstOrDefault();
            if (checkProfile != null)
            {
                checkProfile.FirstName = data.FirstName;
                checkProfile.LastName = data.LastName;
                checkProfile.Address = data.Address;
                checkProfile.Email = data.Email;
                checkProfile.PhoneNumber = data.PhoneNumber;
                checkProfile.Price = data.Price;
                checkProfile.Bio = data.Bio;
                checkProfile.MusicSpecialization = data.MusicSpecialization;
                checkProfile.DateCreated = DateTime.Now;
                _context.Update(checkProfile);
                _context.SaveChanges();
                return true;
            }
            return false;
        }


        public bool UpdateProfilePicture(string base64, string id)
        {
            if (base64 != null && id != null)
            {
                var checkProfilePicture = _context.ApplicationUser.Where(x => x.Id == id && !x.IsDeactivated).FirstOrDefault();
                if (checkProfilePicture != null)
                {
                    checkProfilePicture.Image = base64;

                    _context.Update(checkProfilePicture);
                    _context.SaveChanges();
                    return true;
                }
            }
           
            return false;
        }


        public List<ApplicationUserViewModel> ViewAllUsersInCategory(int categoryId, string loggedInUserId)
        {
            try
            {
                if (loggedInUserId != null && categoryId > 0)
                {
                    var userDetails = _context.ApplicationUser.Where(x => x.Id != loggedInUserId && !x.IsDeactivated && x.CategoryId == categoryId)
                    .Include(b => b.Category).Select(g =>

                        new ApplicationUserViewModel
                        {
                            Image = g.Image,
                            CategoryId = g.CategoryId,
                            CategoryName = g.Category.Name,
                            FirstName = g.FirstName,
                            LastName = g.LastName,
                            UserName = g.UserName,
                            MusicSpecialization = g.MusicSpecialization,
                            Price = g.Price,
                            Id = g.Id,
                            IsAvailable = g.IsAvailable,
                            IsDeactivated = g.IsDeactivated,
                        });
                    var result = userDetails.ToList();
                    return result;
                }
                return null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int GetTotalBooking(string UserId)     
        {
            return _context.BookingForm.Where(x => x.BookedUserId == UserId || x.BookedById == UserId).Count();
        }

        //public int GetTotalBooking(string UserId)
        //{
        //    var jjj = _context.BookingForm.Where(x => x.BookedUserId == UserId && x.BookedById == UserId).ToList();
        //    if (jjj.Any())
        //    {
        //        return jjj.Count();
        //    }
        //    return 0;
        //}

        public int GetTotalApprovedBooking(string UserId)
        {
            return _context.BookingForm.Where(x => x.BookedUserId == UserId && x.BookingStatus == Database.Enum.BookingStatus.Approved).Count();
        }

        public int GetTotalDeclinedBooking(string UserId)
        {
            return _context.BookingForm.Where(x => x.BookedUserId == UserId && x.BookingStatus == Database.Enum.BookingStatus.Declined).Count();
        }

        public int GetTotalCancelledBooking(string UserId)
        {
            return _context.BookingForm.Where(x => x.BookedUserId == UserId && x.BookingStatus == Database.Enum.BookingStatus.Cancel).Count();
        }
        public int GetTotalPendingBooking(string UserId)
        {
            return _context.BookingForm.Where(x => x.BookedById == UserId && x.BookingStatus == Database.Enum.BookingStatus.Pending).Count();
        }
    }
}
