using Bookme.Database;
using Bookme.IHelper;
using Bookme.Models;
using Bookme.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace Bookme.Controllers
{
    public class AccountController : Controller
    {
             private readonly ApplicationDbContext _context;
             private readonly IUserHelper _userHelper;
             private readonly SignInManager<ApplicationUser> _signInManager;
             private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IUserHelper userHelper)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _userHelper = userHelper;
        }
        [HttpGet]
        public IActionResult Registration()
        {
            ViewBag.Gender = _userHelper.DropDownOfGender();
            ViewBag.Category = _userHelper.DropDownOfCategory();
            return View();
        }
        
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Registration(string registrationDetails)
        {
            if (registrationDetails != null)
            {
                var userViewModel = JsonConvert.DeserializeObject<ApplicationUserViewModel>(registrationDetails);
                if (userViewModel != null)
                {
                    var checkForEmail = await _userHelper.FindByEmailAsync(userViewModel.Email).ConfigureAwait(false);
                    if (checkForEmail != null)
                    {
                        return Json(new { isError = true, msg = "Email belongs to another user" });
                    }
                    if (userViewModel.Password != userViewModel.ConfirmPassword)
                    {
                        return Json(new { isError = true, msg = "Password and Confirm password must match" });
                    }
                    var createUser = await _userHelper.CreateUser(userViewModel).ConfigureAwait(false);
                    if (createUser != null)
                    {
                        return Json(new { isError = false, msg = "User registered successfully, login to continue" });
                    }
                    return Json(new { isError = true, msg = "Unable to create User" });
                }
            }
            return Json(new { isError = true, msg = " An error has occurred, try again. Please contact support if the error persists." });
        }
        [HttpPost]
        public JsonResult login(string email, string password)
        {
            if (email != null && password != null) 
            {
                var user = _userHelper.FindByEmailAsync(email).Result;
                if (user != null)
                {
                    var signIn = _signInManager.PasswordSignInAsync(user, password, true, true).Result;
                    if (signIn.Succeeded)
                    {
                        var url = "";
                        var userRole = _userManager.GetRolesAsync(user).Result;
                        if (userRole.Contains("SuperAdmin"))
                        {
                            url = "/SuperAdmin/Index";
                        }
                        else
                        {
                            url = "/Admin/Index";
                        }
                        return Json(new { isError = false, dashboard = url });
                    }
                    return Json(new { isError = true, msg = "Could not sign in" });
                }
                return Json(new { isError = true, msg = "User does not exist" });
            }
            return Json(new { isError = true, msg = "Add email and password" });
        }

        public IActionResult logout()
        {

            _signInManager.SignOutAsync().GetAwaiter().GetResult();
            return RedirectToAction("Index","Home");
        }

        public IActionResult SuperAdminRegistration() 
        {
           return View();
        }

        [HttpPost]
        public async Task<JsonResult> SuperAdminRegistration(string superAdminRegistrationDetails)
        {
            if (superAdminRegistrationDetails != null)
            {
                var superAdminViewModel = JsonConvert.DeserializeObject<ApplicationUserViewModel>(superAdminRegistrationDetails);
                if (superAdminViewModel != null)
                {
                    var checkForEmail = await _userHelper.FindByEmailAsync(superAdminViewModel.Email).ConfigureAwait(false);
                    if (checkForEmail != null)
                    {
                        return Json(new { isError = true, msg = "Email belongs to another user" });
                    }
                    if (superAdminViewModel.Password != superAdminViewModel.ConfirmPassword)
                    {
                        return Json(new { isError = true, msg = "Password and Confirm password must match" });
                    }
                    var createUser = await _userHelper.CreateSuperAdmin(superAdminViewModel).ConfigureAwait(false);
                    if (createUser != null)
                    {
                        return Json(new { isError = false, msg = "SuperAdmin registered successfully, login to continue" });
                    }
                    return Json(new { isError = true, msg = "Unable to create SuperAdmin" });
                }
            }
            return Json(new { isError = true, msg = " An error has occurred, try again. Please contact support if the error persists." });
        }

       
    }
}
