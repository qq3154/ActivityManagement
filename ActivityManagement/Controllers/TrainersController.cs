using ActivityManagement.Models;
using ActivityManagement.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ActivityManagement.Controllers
{
    [Authorize(Roles = "admin")]
    public class TrainersController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        public TrainersController()
        {
            _context = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }

        // GET: Trainers
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ShowTrainers()
        {
            var users = _context.Users.ToList();

            var trainer = new List<ApplicationUser>();

            foreach (var user in users)
            {
                if (_userManager.GetRoles(user.Id)[0].Equals("trainer"))
                {
                    trainer.Add(user);
                }
            }

            return View(trainer);
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