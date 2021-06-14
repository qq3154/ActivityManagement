using ActivityManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ActivityManagement.Controllers
{
	[Authorize]
	public class CoursesController : Controller
	{
		private ApplicationDbContext _context;
		public CoursesController()
		{
			_context = new ApplicationDbContext();
		}
		// GET: Todoes
		public ActionResult Index()
		{
			var courses = _context.Courses.ToList();
			return View(courses);
		}

		public ActionResult Details(int? id)
		{
			if (id == null) return HttpNotFound();

			var courses = _context.Courses.SingleOrDefault(t => t.Id == id);

			if (courses == null) return HttpNotFound();
			return View(courses);
		}
		public ActionResult Create()
		{
			return View();
		}
	}
}