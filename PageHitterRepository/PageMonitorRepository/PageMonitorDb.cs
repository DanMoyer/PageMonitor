using System.Data.Entity.Core.Objects;
using System.Reflection;
using PageMonitorRepository.AdHoc;
using PageMonitorRepository.Identity_Pages;
using PageMonitorRepository.Monitor;

namespace PageMonitorRepository
{
	using System.Data.Entity;

	public class PageMonitorDb : DbContext
	{
		public PageMonitorDb()
			: base("name=PageMonitor")
		{
		}

		public virtual DbSet<Page> Pages { get; set; }
		public virtual DbSet<PageStatus> PageStatus { get; set; }

		public virtual DbSet<Delay.Delay> Delays { get; set; }


		public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
		public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

		public virtual DbSet<AdHocPage> AdHocPages { get; set; }
		public virtual DbSet<AdHocPageStatu> AdHocPageStatus { get; set; }



		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
		}
	}
}
