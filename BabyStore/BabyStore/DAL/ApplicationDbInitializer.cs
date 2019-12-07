using BabyStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BabyStore.DAL
{
    //DropCreateDatabaseAlways
    //DropCreateDatabaseIfModelChanges
    public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            InitializeIdentityForEF(context);
            base.Seed(context);
        }

        //Create User=admin@mvcbabystore.com with Adm1n@mvcbabystore.com in the Admin role
        public static void InitializeIdentityForEF(ApplicationDbContext db)
        {
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
            const string name = "admin@mvcbabystore.com";
            const string password = "Adm1n@mvcbabystore.com";
            const string roleName = "Admin";
            //Create Role Admin if it does not exist
            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new IdentityRole(roleName);
                var roleresult = roleManager.Create(role);
            }
            db.SaveChanges();
            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    FirstName = "Admin",
                    LastName = "AdminVeliki",
                    DateOfBirth = new DateTime(2000,1,1),
                    UserName = name,
                    Email = name,
                    Address = new Models.Adresses.Address()
                    {
                        AddressLine1 = "ulica do mojega",
                        AddressLine2 = "sta te boli k",
                        County = "Vojvodina",
                        Postcode = "21",
                        Town = "Novi Sad"
                    }
                };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }
            db.SaveChanges();
            // Add user admin to Role Admin if not already added
            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                var result = userManager.AddToRole(user.Id, role.Name);
            }
            db.SaveChanges();

            //Add default users role
            const string userRoleName = "Users";
            var roleForUsers = roleManager.FindByName(userRoleName);
            if (roleForUsers == null)
            {
                roleForUsers = new IdentityRole(userRoleName);
                var roleresult = roleManager.Create(roleForUsers);
            }
            db.SaveChanges();
        }
    }
}
