using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActivityManagement.Models;

namespace ActivityManagement.Controllers
{
	[Authorize]
	public class CategoriesController : Controller
    {
        private ApplicationDbContext _context;
        public CategoriesController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Categories
        public ActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

		[HttpGet]
		[Authorize(Roles = "staff")]
		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[Authorize(Roles = "staff")]
		public ActionResult Create(Category category)
		{
			if (!ModelState.IsValid)
			{
				return View(category);
			}

			var newCategory = new Category
			{
				Name = category.Name
			};

			_context.Categories.Add(newCategory);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}