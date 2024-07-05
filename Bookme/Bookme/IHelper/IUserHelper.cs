using Bookme.Models;
using Bookme.ViewModels;

namespace Bookme.IHelper
{
    public interface IUserHelper
    {
        List<CommonDropDown> DropDownOfGender();
        List<Category> DropDownOfCategory();
        Task<ApplicationUser>? FindByEmailAsync(string email);
        Task<ApplicationUser> CreateUser(ApplicationUserViewModel userDetails);

        Task<ApplicationUser> CreateSuperAdmin(ApplicationUserViewModel superAdminDetails);
        //bool CheckIfAvailable(string userId);
        ApplicationUser FindById(string Id);
        string GetCurrentUserId(string username);
    }
}
