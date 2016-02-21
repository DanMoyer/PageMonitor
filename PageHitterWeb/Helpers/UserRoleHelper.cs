using System;
using System.Data.Entity.Validation;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PageHitterWeb.Models;

//  http://stackoverflow.com/questions/15820505/dbentityvalidationexception-how-can-i-easily-tell-what-caused-the-error


namespace PageHitterWeb.Helpers
{
	public class UserRoleHelper
	{

		public static void AddUser(string userName)
		{
			using (var context = new ApplicationDbContext())
			{
				try
				{
					if (context.Users.Any(u => u.UserName == userName)) return;

					var store = new UserStore<ApplicationUser>(context);
					var manager = new UserManager<ApplicationUser>(store);

					var appUser = new ApplicationUser { UserName = userName };
					manager.Create(appUser, "Password.1");
				}
				catch (Exception ex)
				{
					// ReSharper disable once UnusedVariable
					var message = ex.Message;
					throw;
				}
			}
		}


		public static void AddRole(string roleName)
		{
			using (var context = new ApplicationDbContext())
			{
				try
				{
					if (context.Roles.Any(r => r.Name == roleName)) return;

					var roleStore = new RoleStore<IdentityRole>(context);
					var roleManager = new RoleManager<IdentityRole>(roleStore);

					var role = new IdentityRole(roleName);

					roleManager.Create(role);
				}
				catch (Exception ex)
				{
					// ReSharper disable once UnusedVariable
					var message = ex.Message;
					throw;
				}
			}
		}

		public static void AddUserRole(string userName, string roleName)
		{
			using (var context = new ApplicationDbContext())
			{
				try
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
				catch (DbEntityValidationException ex)
				{
					// Retrieve the error messages as a list of strings.

					// ReSharper disable once UnusedVariable
					var errorMessages = ex.EntityValidationErrors
						.SelectMany(x => x.ValidationErrors)
						.Select(x => x.ErrorMessage);

					throw;
				}
			}
		}
	}
}