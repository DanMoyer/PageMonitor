using System.Reflection;

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

		public virtual DbSet<Delay> Delays { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
		}
	}
}
