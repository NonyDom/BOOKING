using Bookme.Database;
using Bookme.IHelper;
using Bookme.Models;
using Bookme.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace Bookme.Helper
{
    public class UserHelper : IUserHelper
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserHelper(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<CommonDropDown> DropDownOfGender()
        {
            try
            {
                var common = new CommonDropDown()
                {
                    Id = 0,
                    Name = "Select Gender"
                };
                var genderList = _context.CommonDropown.Where(x => x.Id > 0 && !x.Deleted).ToList();
                var drp = genderList.Select(x => new CommonDropDown
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToList();
                drp.Insert(0, common);
                return drp;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        public List<Category> DropDownOfCategory()
        {
            try
            {
                var common = new Category()
                {
                    Id = 0,
                    Name = "Select Category"
                };
                var categoryList = _context.Category.Where(x => x.Id > 0 && !x.Deleted).ToList();
                var drp = categoryList.Select(x => new Category
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToList();
                drp.Insert(0, common);
                return drp;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        public async Task<ApplicationUser>? FindByEmailAsync(string email)
        {
            try
            {
                var user = await _context.ApplicationUser
                    .Where(s => s.Email == email && !s.IsDeactivated)
                    .FirstOrDefaultAsync().ConfigureAwait(false);
                if (user != null)
                {
                    return user;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return null;
        }
        public async Task<ApplicationUser> CreateUser(ApplicationUserViewModel userDetails, string base64)
        {
            var user = new ApplicationUser();
            user.UserName = userDetails.UserName;
            user.Image = base64;
            user.Email = userDetails.Email;
            user.FirstName = userDetails.FirstName;
            user.LastName = userDetails.LastName;
            user.PhoneNumber = userDetails.PhoneNumber;
            user.MusicSpecialization = userDetails.MusicSpecialization;
            user.Address = userDetails.Address;
            user.DateCreated = DateTime.Now;
            user.IsDeactivated = false;
            user.IsAvailable = true;
            user.Bio = userDetails.Bio;
            user.CategoryId = userDetails.CategoryId;
            user.GenderId = userDetails.GenderId;
            user.Price = userDetails.Price;

            var createUser = await _userManager.CreateAsync(user, userDetails.Password).ConfigureAwait(false);
            if (createUser.Succeeded)
            {
                var addRole = await _userManager.AddToRoleAsync(user, "Admin").ConfigureAwait(false);
                return user;
            }
            return null;
        }

        public async Task<ApplicationUser> CreateSuperAdmin(ApplicationUserViewModel superAdminDetails)
        {
            var superAdmin = new ApplicationUser();
            superAdmin.Email = superAdminDetails.Email;
            superAdmin.PhoneNumber = superAdminDetails.PhoneNumber;
            superAdmin.UserName = superAdminDetails.UserName;
            superAdmin.FirstName = superAdminDetails.FirstName;
            superAdmin.LastName = superAdminDetails.LastName;
            superAdmin.DateCreated = DateTime.Now;
            superAdmin.IsDeactivated = false;
            superAdmin.IsAvailable = true;
            var createUser = await _userManager.CreateAsync(superAdmin, superAdminDetails.Password).ConfigureAwait(false);
            if (createUser.Succeeded)
            {
                var addRole = await _userManager.AddToRoleAsync(superAdmin, "SuperAdmin").ConfigureAwait(false);
                return superAdmin;
            }
            return null;
        }

        //public bool CheckIfAvailable(string userId)
        //{
        //    if (userId != null )
        //    {
        //        var checkIfavailable = _context.ApplicationUser.Where(x => x.Id == userId && !x.IsDeactivated && x.IsAvailable == true)
        //            .FirstOrDefault();
        //        if (checkIfavailable != null)
        //        {
        //            return true;
        //        }
        //        return false;
        //    }
        //    return false;
        //}
        public ApplicationUser FindById(string Id)
        {
            return _context.ApplicationUser.Where(s => s.Id == Id)
                .Include(j => j.Category).FirstOrDefault();
        }
        public string GetCurrentUserId(string username)
        {
            return _userManager.Users.Where(s => s.UserName == username)?.FirstOrDefaultAsync().Result.Id?.ToString();
        }

        
    }
}
