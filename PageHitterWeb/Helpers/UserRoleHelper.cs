using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PageHitterWeb.Models;


namespace PageHitterWeb.Helpers
{
	public class UserRoleHelper
	{

		public static void AddUser(ApplicationDbContext context, string userName)
		{
			if (context.Users.Any(u => u.UserName == userName)) return;

			var store = new UserStore<ApplicationUser>(context);
			var manager = new UserManager<ApplicationUser>(store);

			var appUser = new ApplicationUser { UserName = userName };
			manager.Create(appUser, "Password.1");
		}

		public static void AddRole(ApplicationDbContext context, string roleName)
		{
			if (context.Roles.Any(r => r.Name == roleName)) return;

			var roleStore = new RoleStore<IdentityRole>(context);
			var roleManager = new RoleManager<IdentityRole>(roleStore);

			var role = new IdentityRole(roleName);

			roleManager.Create(role);
		}

		public static void AddUserRole(ApplicationDbContext context, string userName, string roleName)
		{
			if (!context.Roles.Any(r => r.Name == roleName)) return;

			var roleStore = new RoleStore<IdentityRole>(context);
			var roleManager = new RoleManager<IdentityRole>(roleStore);

			var store = new UserStore<ApplicationUser>(context);
			var userManager = new UserManager<ApplicationUser>(store);

			var user = userManager.FindByName(userName);
			var role = roleManager.FindByName(roleName);

			if (userManager.IsInRole(user.Id, role.Name)) return;

			userManager.AddToRole(user.Id, role.Name);
			context.SaveChanges();
		}


	}
}