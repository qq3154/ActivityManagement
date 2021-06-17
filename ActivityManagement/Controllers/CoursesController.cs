using ActivityManagement.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

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

		// GET: Courses
		[HttpGet]
		public ActionResult Index()
		{
			var courses = _context.Courses.ToList();
			return View(courses);
		}


		[HttpGet]
		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Create(Course course)
		{
			if (!ModelState.IsValid)
			{
				return View(course);
			}

			var newCourse = new Course
			{
				Name = course.Name
			};

			_context.Courses.Add(newCourse);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}

		[HttpGet]
		public ActionResult Members(int? id)
		{
			if (id == null)
				return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
			var members = _context.CoursesUsers
				.Include(t => t.User)
				.Where(t => t.CourseId == id)
				.Select(t => t.User);
			ViewBag.TeamId = id;
			return View(members);
		}

	}
}