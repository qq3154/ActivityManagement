using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
		[Authorize(Roles = "staff")]
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
				Name = category.Name,
				Description = category.Description
			};

			_context.Categories.Add(newCategory);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}


		[HttpGet]
		[Authorize(Roles = "staff")]
		public ActionResult Details(int? id)
		{
			if (id == null) return HttpNotFound();

			var category = _context.Categories				
				.SingleOrDefault(t => t.Id == id);

			if (category == null) return HttpNotFound();

			return View(category);
		}


		[HttpGet]
		[Authorize(Roles = "staff")]
		public ActionResult Edit(int? id)
		{
			if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			//var userId = User.Identity.GetUserId();

			var category = _context.Categories
				//.Where(t => t.UserId.Equals(userId))
				.SingleOrDefault(t => t.Id == id);

			if (category == null) return HttpNotFound();

			/*var viewModel = new CourseCategoriesViewModel()
			{
				Course = course,
				Categories = _context.Categories.ToList()
			};*/

			return View(category);
		}

		[HttpPost]
		[Authorize(Roles = "staff")]
		public ActionResult Edit(Category category)
		{
			if (!ModelState.IsValid)
			{
				/*var viewModel = new CourseCategoriesViewModel()
				{
					Course = course,
					Categories = _context.Categories.ToList()
				};*/
				return View(category);
			}

			//var userId = User.Identity.GetUserId();

			var categoryInDb = _context.Categories
				//.Where(t => t.UserId.Equals(userId))
				.SingleOrDefault(t => t.Id == category.Id);

			if (categoryInDb == null) return HttpNotFound();
			categoryInDb.Name = category.Name;
			categoryInDb.Description = category.Description;

			_context.SaveChanges();

			return RedirectToAction("Index");
		}

		[Authorize(Roles = "staff")]
		public ActionResult Delete(int? id)
		{
			if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			//var userId = User.Identity.GetUserId();

			var category = _context.Categories
				//.Where(t => t.UserId.Equals(userId))
				.SingleOrDefault(t => t.Id == id);

			if (category == null) return HttpNotFound();

			_context.Categories.Remove(category);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}
	}
}