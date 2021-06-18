using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActivityManagement.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ActivityManagement.ViewModels;
using System.Diagnostics;

namespace ActivityManagement.Controllers
{
	[Authorize]
	public class CoursesController : Controller
	{
		private ApplicationDbContext _context;
		private UserManager<ApplicationUser> _userManager;

		public CoursesController()
		{
			_context = new ApplicationDbContext();
			_userManager = new UserManager<ApplicationUser>(
				new UserStore<ApplicationUser>(new ApplicationDbContext()));
		}

		// GET: Courses
		[HttpGet]
		[Authorize(Roles = "staff")]
		public ActionResult Index()
		{
			var courses = _context.Courses.ToList();
			return View(courses);
		}


		[HttpGet]
		[Authorize(Roles = "staff")]
		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[Authorize(Roles = "staff")]
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
		[Authorize(Roles = "staff")]
		public ActionResult Members(int? id)
		{
			if (id == null)
				return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
			var members = _context.CoursesUsers
				.Include(t => t.User)
				.Where(t => t.CourseId == id)		
				.Select(t => t.User);
			var trainee = new List<ApplicationUser>();       // Init List Users to Add Course

			foreach (var user in members)
			{
				if (_userManager.GetRoles(user.Id)[0].Equals("trainee"))
				{
					trainee.Add(user);
				}
			}
			ViewBag.CourseId = id;
			return View(trainee);
		}

		[HttpGet]
		[Authorize(Roles = "staff")]
		public ActionResult AddMembers(int? id)
		{
			if (id == null)
				return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

			if (_context.Courses.SingleOrDefault(t => t.Id == id) == null)
				return HttpNotFound();

			var usersInDb = _context.Users.ToList();      // User trong Db

			var usersInCourse = _context.CoursesUsers         // User trong Course
				.Include(t => t.User)
				.Where(t => t.CourseId == id)
				.Select(t => t.User)
				.ToList();

			var usersToAdd = new List<ApplicationUser>();       // Init List Users to Add Course

			foreach (var user in usersInDb)
			{
				if (!usersInCourse.Contains(user)  &&
					_userManager.GetRoles(user.Id)[0].Equals("trainee") )
				{
					usersToAdd.Add(user);
				}
			}

			var viewModel = new CoursesUsersViewModel
			{
				CourseId = (int)id,
				Users = usersToAdd
			};
			return View(viewModel);
		}

		[HttpPost]
		[Authorize(Roles = "staff")]
		public ActionResult AddMembers(CourseUser model)
		{
			var courseUser = new CourseUser
			{
				CourseId = model.CourseId,
				UserId = model.UserId
			};

			_context.CoursesUsers.Add(courseUser);
			_context.SaveChanges();

			return RedirectToAction("Members", new { id = model.CourseId });
		}

		[HttpGet]
		[Authorize(Roles = "staff")]
		public ActionResult RemoveMember(int id, string userId)
		{
			var courseUserToRemove = _context.CoursesUsers
				.SingleOrDefault(t => t.CourseId == id && t.UserId == userId);

			if (courseUserToRemove == null)
				return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

			_context.CoursesUsers.Remove(courseUserToRemove);
			_context.SaveChanges();
			return RedirectToAction("Members", new { id = id });
		}

		[Authorize(Roles = "trainer, trainee")]
		[HttpGet]
		public ActionResult Mine()
		{
			var userId = User.Identity.GetUserId();

			var courses = _context.CoursesUsers
				.Where(t => t.UserId.Equals(userId))
				.Include(t => t.Course)
				.Select(t => t.Course)
				.ToList();

			return View(courses);
		}

		public ActionResult Details(int? id)
		{
			if (id == null) return HttpNotFound();

			var course = _context.Courses.SingleOrDefault(t => t.Id == id);

			if (course == null) return HttpNotFound();

			return View(course);
		}

	}
}