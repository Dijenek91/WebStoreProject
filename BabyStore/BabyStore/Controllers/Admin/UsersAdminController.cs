using BabyStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Net;
using ViewModel = BabyStore.ViewModels.Admin;
using BabyStore.ViewModels.Admin;

namespace BabyStore.Controllers.Admin
{
    [Authorize(Roles="Admin")]
    public class UsersAdminController : Controller
    {
        #region Properties

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ??
                HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }
        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        #endregion

        #region Constructor
        public UsersAdminController()
        {
        }

        public UsersAdminController(ApplicationUserManager userManager, ApplicationRoleManager
        roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }
        #endregion

        //
        // GET: /UsersAdmin/
        public async Task<ActionResult> Index()
        {
            var users = await UserManager.Users.ToListAsync();
            return View(users);
        }

        //
        // GET: /UsersAdmin/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if(string.IsNullOrWhiteSpace(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await UserManager.FindByIdAsync(id);
            if(user == null)
            {
                return HttpNotFound();
            }

            ViewBag.RoleNames = await UserManager.GetRolesAsync(user.Id);
            return View(user);
        }

        //
        // GET: /UsersAdmin/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
            return View();
        }

        //
        // POST: /UsersAdmin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ViewModel.RegisterViewModel registerViewModel, params string[] selectedRoles)
        {
            if(ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = registerViewModel.Email,
                    Email = registerViewModel.Email,
                    DateOfBirth = registerViewModel.DateOfBirth,
                    FirstName = registerViewModel.FirstName,
                    LastName = registerViewModel.LastName,
                    Address = registerViewModel.Address
                };

                var userResult = await UserManager.CreateAsync(user);

                if(userResult.Succeeded)
                {
                    if(selectedRoles != null)
                    {
                        var result = await UserManager.AddToRolesAsync(user.Id, selectedRoles);
                        if(!result.Succeeded)
                        {
                            ModelState.AddModelError("", result.Errors.First());
                            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
                            return View();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", userResult.Errors.First());
                    ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
                    return View();
                }
                return RedirectToAction("Index");
            
            }

            ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
            return View();
            
        }

        //
        // GET: /UsersAdmin/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if(string.IsNullOrWhiteSpace(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await UserManager.FindByIdAsync(id);
            if(user == null)
            {
                return HttpNotFound();
            }

            var userRoles = await UserManager.GetRolesAsync(user.Id);

            EditUserViewModel editUserViewModel = new EditUserViewModel()
            {
                Id = user.Id,
                Address = user.Address,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem() 
                {
                    Selected = userRoles.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                })
            };

            return View(editUserViewModel);
        }

        //
        // POST: /UsersAdmin/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(EditUserViewModel editUserViewModel, params string[] selectedRoles)
        {
           if(ModelState.IsValid)
           {
               var user = await UserManager.FindByIdAsync(editUserViewModel.Id);
               if(user == null)
               {
                   return HttpNotFound();
               }

               user.FirstName = editUserViewModel.FirstName;
               user.LastName = editUserViewModel.LastName;
               user.DateOfBirth = editUserViewModel.DateOfBirth;
               user.Address = editUserViewModel.Address;

               var alreadyDefinedUserRoles = await UserManager.GetRolesAsync(user.Id);

               selectedRoles = selectedRoles ?? new string[] { };

               //add new roles
               var result = await UserManager.AddToRolesAsync(user.Id, selectedRoles.Except(alreadyDefinedUserRoles).ToArray<string>());
               if(!result.Succeeded)
               {
                   ModelState.AddModelError("", result.Errors.First());
                   return View();
               }
               
               //remove old ones
               result = await UserManager.RemoveFromRolesAsync(user.Id, alreadyDefinedUserRoles.Except(selectedRoles).ToArray<string>());
               if (!result.Succeeded)
               {
                   ModelState.AddModelError("", result.Errors.First());
                   return View();
               }

               return RedirectToAction("Index");
           }
           
           ModelState.AddModelError("", "Something FAILED");
           return View();
        }

        //
        // GET: /UsersAdmin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /UsersAdmin/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
