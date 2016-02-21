namespace PageMonitorRepository.Migrations
{
	using System;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;

	internal sealed class Configuration : DbMigrationsConfiguration<PageMonitorRepository.PageMonitorDb>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
		}

		protected override void Seed(PageMonitorRepository.PageMonitorDb context)
		{
			//  This method will be called after migrating to the latest version.

			//  You can use the DbSet<T>.AddOrUpdate() helper extension method 
			//  to avoid creating duplicate seed data. E.g.
			//
			//    context.People.AddOrUpdate(
			//      p => p.FullName,
			//      new Person { FullName = "Andrew Peters" },
			//      new Person { FullName = "Brice Lambson" },
			//      new Person { FullName = "Rowan Miller" }
			//    );
			//



			if (System.Diagnostics.Debugger.IsAttached == false)
				System.Diagnostics.Debugger.Launch();

			//UserRoleHelper.AddUser(context, "moyerd1@findlay.edu");
			//UserRoleHelper.AddUser(context, "testsecadmin@findlay.edu");

			//UserRoleHelper.AddRole(context, "admin");
			//UserRoleHelper.AddRole(context, "securityadmin");

			//UserRoleHelper.AddUserRole(context, "moyerd1@findlay.edu", "admin");
			//UserRoleHelper.AddUserRole(context, "moyerd1@findlay.edu", "securityadmin");
			//UserRoleHelper.AddUserRole(context, "testsecadmin@findlay.edu", "securityadmin");



		}
	}
}
