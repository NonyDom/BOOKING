using Bookme.Database;
using Bookme.IHelper;
using Bookme.Models;
using Bookme.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using System.ComponentModel.Design;

namespace Bookme.Controllers
{
    public class SuperAdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAdminHelper _adminHelper;

        public SuperAdminController(ApplicationDbContext context, IAdminHelper adminHelper)
        {
            _context = context;
            _adminHelper = adminHelper;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Category() 
        {
            var categories = _context.Category.Where(p => p.Active && !p.Deleted).ToList();
            if (categories.Count > 0)
            {
                var cat = new CategoryViewModel
                {
                    Categories = categories,
                };
                return View(cat);
            }
            return View();
        }
        [HttpPost]
        public IActionResult CreateCategory(string categoryName)
        {
            if (!string.IsNullOrEmpty(categoryName))
            {
                var check = _context.Category.Where(f => f.Name.ToLower() == categoryName.ToLower() && !f.Deleted).FirstOrDefault();
                if (check != null)
                {
                    return Json(new { isError = true, msg = "Name Already Exists" });
                }

                if (ModelState.IsValid)
                {
                    var model = new Category()
                    {
                        DateCreated = DateTime.Now,
                        Deleted = false,
                        Name = categoryName,
                        Active = true,
                    };

                    _context.Category.Add(model);
                    _context.SaveChanges();
                    return Json(new { isError = false, msg = "Category created successfully" });
                }
                return Json(new { isError = true, msg = "Failed" });
            }
            return Json(new { isError = true, msg = "Failed" });
        }
        [HttpGet]
        public JsonResult GetCategoryToEdit(int categoryId) 
        {
            if (categoryId != 0)
            {
                var category = _adminHelper.GetCategoryById(categoryId);
                if (category != null) 
                {
                    CategoryViewModel categoryView = new CategoryViewModel()
                    {
                        Id = category.Id,
                        Name = category.Name,
                    };
                    return Json(new { isError = false, data = categoryView });
                }
            }
            return Json(new { isError = true, msg = "Error occured" });
        }
        [HttpPost]
        public JsonResult UpdateCategory(string categoryName)
        {
            if (categoryName == null)
            {
                return Json(new { isError = true, msg = "" });
            }
            var data = JsonConvert.DeserializeObject<CategoryViewModel>(categoryName);
            {
               var category = _adminHelper.UpdateCategoryName(data);
                if (category == true)
                {
                    return Json(new { isError = false, msg = "Category updateed successfully" });
                }
            }
            return Json(new { isError = true, msg = "Error occured" });
        }
        public JsonResult DeleteCategory(int id)
        {
            if (id != 0)
            {
                var category = _context.Category.Where(x => x.Id == id && x.Active).FirstOrDefault();
                if (category != null)
                {
                   category.Active = false;
                   category.Deleted = true;
                    _context.Update(category);
                    _context.SaveChanges();
                    return Json(new { isError = false, msg = "Category deleted successfully" });
                }
            }
            return Json(new { isError = true, msg = "Could not find Category" });
        }
    }
}
