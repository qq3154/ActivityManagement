using ActivityManagement.Models;
using ActivityManagement.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ActivityManagement.Controllers
{
    [Authorize(Roles = "admin")]
    public class StaffsController : Controller
    {
		
		private ApplicationDbContext _context;
		private UserManager<ApplicationUser> _userManager;
		public StaffsController()
		{
			_context = new ApplicationDbContext();
			_userManager = new UserManager<ApplicationUser>(
				new UserStore<ApplicationUser>(new ApplicationDbContext()));
		}

		// GET: Staffs
		public ActionResult Index()
        {
            return View();
        }

		[HttpGet]
		public ActionResult ShowStaffs()
		{
			var users = _context.Users.ToList();

			var staffs = new List<ApplicationUser>();

			foreach (var user in users)
			{
				if (_userManager.GetRoles(user.Id)[0].Equals("staff"))
				{
					staffs.Add(user);
				}
			}

			return View(staffs);
		}

		[HttpGet]
		public ActionResult Edit(string id)
		{
			

			var userInfo = _context.UsersInfos				
				.SingleOrDefault(u => u.UserId.Equals(id));

			if (userInfo == null) return HttpNotFound();

			return View(userInfo);
		}
		[HttpPost]
		public ActionResult Edit(UserInfo userInfo)
		{
			var userInfoInDb = _context.UsersInfos.SingleOrDefault(u => u.UserId.Equals(userInfo.UserId));

			if (userInfo == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			userInfoInDb.FullName = userInfo.FullName;
			userInfoInDb.Age = userInfo.Age;
			userInfoInDb.TOEICScore = userInfo.TOEICScore;
			userInfoInDb.ProgrammingLanguage = userInfo.ProgrammingLanguage;
			_context.SaveChanges();

			return RedirectToAction("Index");
		}

		[HttpGet]
		public ActionResult Details(string id)
		{		
			var user = _context.UsersInfos
				.SingleOrDefault(t => t.UserId == id);

			if (user == null) return HttpNotFound();

			return View(user);
		}

		public ActionResult Delete(string id)
		{
			if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			//var userId = User.Identity.GetUserId();
			var userInfo = _context.UsersInfos
				.SingleOrDefault(t => t.UserId == id);
			var user = _context.Users
				.SingleOrDefault(t => t.Id == id);

			if (user == null) return HttpNotFound();
			_context.UsersInfos.Remove(userInfo);
			_context.Users.Remove(user);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}
		[HttpGet]
		public ActionResult ChangePassword(string id)
		{
			//string password;
			var viewModel = new ChangePasswordsViewModel()
			{
				Password = "",
				ConfirmPassword = "",
				UserId = id
			};
			return View(viewModel);
		}
		[HttpPost]
		public async System.Threading.Tasks.Task<ActionResult> ChangePassword(ChangePasswordsViewModel changepassword)
		{
			var user = await _userManager.FindByIdAsync(changepassword.UserId);
			await _userManager.RemovePasswordAsync(changepassword.UserId);
			await _userManager.AddPasswordAsync(user.Id, changepassword.Password);
			return RedirectToAction("Index");
		}

	}
}