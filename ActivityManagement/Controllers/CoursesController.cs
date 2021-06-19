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
using System.Net;

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
			var viewModel = new CourseCategoriesViewModel()
			{
				Categories = _context.Categories.ToList()
			};
			return View(viewModel);
		}

		[HttpPost]
		[Authorize(Roles = "staff")]
		public ActionResult Create(Course course)
		{
			if (!ModelState.IsValid)
			{
				var viewModel = new CourseCategoriesViewModel()
				{
					Course = course,
					Categories = _context.Categories.ToList()
				};
				return View(viewModel);
			}

			var newCourse = new Course
			{
				Name = course.Name,
				CategoryId = course.CategoryId,
				StartDate = course.StartDate,
				Description = course.Description
			};

			_context.Courses.Add(newCourse);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}


		[HttpGet]
		public ActionResult Details(int? id)
		{
			if (id == null) return HttpNotFound();

			var course = _context.Courses
				.Include(t => t.Category)
				.SingleOrDefault(t => t.Id == id);

			if (course == null) return HttpNotFound();

			return View(course);
		}
		

		[HttpGet]
		public ActionResult Edit(int? id)
		{
			if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			//var userId = User.Identity.GetUserId();

			var course = _context.Courses
				//.Where(t => t.UserId.Equals(userId))
				.SingleOrDefault(t => t.Id == id);

			if (course == null) return HttpNotFound();

			var viewModel = new CourseCategoriesViewModel()
			{
				Course = course,
				Categories = _context.Categories.ToList()
			};

			return View(viewModel);
		}

		[HttpPost]
		public ActionResult Edit(Course course)
		{
			if (!ModelState.IsValid)
			{
				var viewModel = new CourseCategoriesViewModel()
				{
					Course = course,
					Categories = _context.Categories.ToList()
				};
				return View(viewModel);
			}

			//var userId = User.Identity.GetUserId();

			var courseInDb = _context.Courses
				//.Where(t => t.UserId.Equals(userId))
				.SingleOrDefault(t => t.Id == course.Id);

			if (courseInDb == null) return HttpNotFound();
			courseInDb.Name = courseInDb.Name;
			courseInDb.Description = courseInDb.Description;
			courseInDb.StartDate = courseInDb.StartDate;
			courseInDb.CategoryId = courseInDb.CategoryId;

			_context.SaveChanges();

			return RedirectToAction("Index");
		}

		public ActionResult Delete(int? id)
		{
			if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			//var userId = User.Identity.GetUserId();

			var course = _context.Courses
				//.Where(t => t.UserId.Equals(userId))
				.SingleOrDefault(t => t.Id == id);

			if (course == null) return HttpNotFound();

			_context.Courses.Remove(course);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}				

		[HttpGet]
		[Authorize(Roles = "staff")]
		public ActionResult ShowTrainees(int? id)
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
		public ActionResult AddTrainees(int? id)
		{
			if (id == null)
				return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

			if (_context.Courses.SingleOrDefault(t => t.Id == id) == null)
				return HttpNotFound();

			var usersInDb = _context.Users.ToList();      // User trong Db

			var usersInTeam = _context.CoursesUsers         // User trong Team
				.Include(t => t.User)
				.Where(t => t.CourseId == id)
				.Select(t => t.User)
				.ToList();

			var usersToAdd = new List<ApplicationUser>();       // Init List Users to Add Team

			foreach (var user in usersInDb)
			{
				if (!usersInTeam.Contains(user) &&
					_userManager.GetRoles(user.Id)[0].Equals("trainee"))
				{
					usersToAdd.Add(user);
				}
			}

			var viewModel = new CourseUsersViewModel
			{
				CourseId = (int)id,
				Users = usersToAdd
			};
			return View(viewModel);
		}

		[HttpPost]
		[Authorize(Roles = "staff")]
		public ActionResult AddTrainees(CourseUser model)
		{
			var courseUser = new CourseUser
			{
				CourseId = model.CourseId,
				UserId = model.UserId
			};

			_context.CoursesUsers.Add(courseUser);
			_context.SaveChanges();

			return RedirectToAction("ShowTrainees", new { id = model.CourseId });
		}

		[HttpGet]
		[Authorize(Roles = "staff")]
		public ActionResult ShowTrainers(int? id)
		{
			if (id == null)
				return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
			var members = _context.CoursesUsers
				.Include(t => t.User)
				.Where(t => t.CourseId == id)
				.Select(t => t.User);
			var trainer = new List<ApplicationUser>();       // Init List Users to Add Course

			foreach (var user in members)
			{
				if (_userManager.GetRoles(user.Id)[0].Equals("trainer"))
				{
					trainer.Add(user);
				}
			}
			ViewBag.CourseId = id;
			return View(trainer);
		}

		[HttpGet]
		[Authorize(Roles = "staff")]
		public ActionResult AddTrainers(int? id)
		{
			if (id == null)
				return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

			if (_context.Courses.SingleOrDefault(t => t.Id == id) == null)
				return HttpNotFound();

			var usersInDb = _context.Users.ToList();      // User trong Db

			var usersInTeam = _context.CoursesUsers         // User trong Team
				.Include(t => t.User)
				.Where(t => t.CourseId == id)
				.Select(t => t.User)
				.ToList();

			var usersToAdd = new List<ApplicationUser>();       // Init List Users to Add Team

			foreach (var user in usersInDb)
			{
				if (!usersInTeam.Contains(user) &&
					_userManager.GetRoles(user.Id)[0].Equals("trainer"))
				{
					usersToAdd.Add(user);
				}
			}

			var viewModel = new CourseUsersViewModel
			{
				CourseId = (int)id,
				Users = usersToAdd
			};
			return View(viewModel);
		}

		[HttpPost]
		[Authorize(Roles = "staff")]
		public ActionResult AddTrainers(CourseUser model)
		{
			var courseUser = new CourseUser
			{
				CourseId = model.CourseId,
				UserId = model.UserId
			};

			_context.CoursesUsers.Add(courseUser);
			_context.SaveChanges();

			return RedirectToAction("ShowTrainers", new { id = model.CourseId });
		}

		[HttpGet]
		[Authorize(Roles = "staff")]
		public ActionResult RemoveTrainees(int id, string userId)
		{
			var courseUserToRemove = _context.CoursesUsers
				.SingleOrDefault(t => t.CourseId == id && t.UserId == userId);

			if (courseUserToRemove == null)
				return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

			_context.CoursesUsers.Remove(courseUserToRemove);
			_context.SaveChanges();
			
			return RedirectToAction("ShowTrainees", new { id = id });
		}

		[HttpGet]
		[Authorize(Roles = "staff")]
		public ActionResult RemoveTrainers(int id, string userId)
		{
			var courseUserToRemove = _context.CoursesUsers
				.SingleOrDefault(t => t.CourseId == id && t.UserId == userId);

			if (courseUserToRemove == null)
				return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

			_context.CoursesUsers.Remove(courseUserToRemove);
			_context.SaveChanges();
			return RedirectToAction("ShowTrainers", new { id = id });
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
	}
}