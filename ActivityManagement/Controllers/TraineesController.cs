using ActivityManagement.Models;
using ActivityManagement.ViewModels;
using Microsoft.Ajax.Utilities;
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
    [Authorize(Roles = "staff")]
    public class TraineesController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        public TraineesController()
        {
            _context = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }

        // GET: Trainees
        public ActionResult Index(string searchString)
        {
            var users = _context.Users.ToList();
            if (!searchString.IsNullOrWhiteSpace())
            {
                users = users.Where(t => t.UserName.Contains(searchString)).ToList();

            }

            var trainees = new List<ApplicationUser>();

            foreach (var user in users)
            {
                if (_userManager.GetRoles(user.Id)[0].Equals("trainee"))
                {
                    trainees.Add(user);
                }
            }

            return View(trainees);
        }

        [HttpGet]
        public ActionResult ShowTrainees()
        {
            var users = _context.Users.ToList();

            var trainee = new List<ApplicationUser>();

            foreach (var user in users)
            {
                if (_userManager.GetRoles(user.Id)[0].Equals("trainee"))
                {
                    trainee.Add(user);
                }
            }

            return View(trainee);
        }

        [HttpGet]
        public ActionResult Details(string id)
        {
            var user = _context.UsersInfos
                .SingleOrDefault(t => t.UserId == id);

            if (user == null) return HttpNotFound();

            return View(user);
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