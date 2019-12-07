using BabyStore.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using BabyStore.ViewModels.Admin;

namespace BabyStore.Controllers.Users
{
    [Authorize]
    public class ManageController : Controller
    {
        public ManageController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(System.Web.HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>())))
        {
        }

        public ManageController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }
        
        //
        // GET: /Manage/
        public async Task<ActionResult> Index()
        {
            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);
            return View(user);
        }

        // GET: /Manage/Edit
        public async Task<ActionResult> Edit()
        {
            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);
            var model = new EditUserViewModel
            {
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address
            };
            return View(model);
        }

        //
        // POST: /Manage/Edit/5
        [HttpPost, ActionName("Edit")]
        public async Task<ActionResult> EditPost()
        {
            var userId = User.Identity.GetUserId();
            var userToUpdate = await UserManager.FindByIdAsync(userId);
            if(TryUpdateModel(userToUpdate, "", new string[] {
                "FirstName",
                "LastName",
                "DateOfBirth",
                "Address" }))
            { 
                 await UserManager.UpdateAsync(userToUpdate);
                 return RedirectToAction("Index");
            }
            return View();
        }

    
	}
}